'use client'

import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useEffect, useMemo, useState } from 'react'

import { DataTable, type ColumnDef } from '@/components/shared/data-table'
import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { ListToolbar } from '@/components/shared/list-toolbar'
import { SelectField } from '@/components/shared/select-field'
import { PageHeader } from '@/components/shared/page-header'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiGet, apiPost } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { filterListByQuery } from '@/lib/filter-list'
import {
  STATUS_TYPE_OPTIONS,
  statusTypeLabel,
  type StatusTypeField,
} from '@/lib/status-type'
import { useTenant } from '@/providers/tenant-provider'

interface StatusReasonDto {
  id: string
  type: StatusTypeField
  name: string
}

const PAGE_SIZE = 10

export default function AppTicketStatusReasonsPage() {
  const router = useRouter()
  const { organizationId } = useTenant()
  const [pageIndex, setPageIndex] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<StatusReasonDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({ type: '1', name: '' })
  const [search, setSearch] = useState('')
  const fetchKey = `${organizationId}-${pageIndex}`

  const filteredData = useMemo(
    () => filterListByQuery(data, search, ['name']),
    [data, search],
  )
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<StatusReasonDto>>(
      `api/organizations/${organizationId}/ticket-status-reasons?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
    )
      .then((page) => {
        if (cancelled) {
          return
        }
        setData(page.items ?? [])
        setTotalItemCount(page.totalItemCount)
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        if (err.statusCode === 404) {
          setData([])
          setTotalItemCount(0)
          setError(null)
          setLoadedKey(fetchKey)
          return
        }

        setError(formatApiErrorMessage(err, 'Erro ao carregar motivos de status.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, pageIndex])

  async function handleCreate() {
    setIsSubmitting(true)
    setError(null)

    try {
      const created = await apiPost<StatusReasonDto>(
        `api/organizations/${organizationId}/ticket-status-reasons`,
        {
          type: Number(createForm.type),
          name: createForm.name.trim(),
        },
      )
      setShowCreate(false)
      setCreateForm({ type: '1', name: '' })
      router.push(`/app/ticket-status-reasons/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar motivo de status.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  const columns: ColumnDef<StatusReasonDto>[] = [
    {
      id: 'type',
      header: 'Tipo',
      cell: (row) => statusTypeLabel(row.type),
    },
    {
      id: 'name',
      header: 'Nome',
      cell: (row) => (
        <Link href={`/app/ticket-status-reasons/${row.id}`} className="font-medium hover:underline">
          {row.name}
        </Link>
      ),
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <Button variant="outline" size="sm" asChild>
          <Link href={`/app/ticket-status-reasons/${row.id}`}>Detalhes</Link>
        </Button>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Motivos de status"
        subtitle="Motivos usados nas transições de status dos tickets."
        actions={
          <Button onClick={() => setShowCreate((value) => !value)}>
            {showCreate ? 'Cancelar' : 'Novo motivo'}
          </Button>
        }
      />

      <FormFeedback error={!showCreate ? error : null} />

      {showCreate ? (
        <Card>
          <CardHeader>
            <CardTitle>Novo motivo de status</CardTitle>
            <CardDescription>Cadastro de motivo para atualização do ticket.</CardDescription>
          </CardHeader>
          <CardContent>
            <FormContainer
              onSubmit={handleCreate}
              isSubmitting={isSubmitting}
              submitLabel="Criar"
              error={error}
            >
              <div className="grid gap-4 sm:grid-cols-2">
                <SelectField
                  id="type"
                  label="Tipo"
                  value={createForm.type}
                  onChange={(value) => setCreateForm((prev) => ({ ...prev, type: value }))}
                  options={[...STATUS_TYPE_OPTIONS]}
                  placeholder="Selecione o tipo"
                  required
                />
                <div className="space-y-2">
                  <Label htmlFor="name">Nome</Label>
                  <Input
                    id="name"
                    value={createForm.name}
                    onChange={(event) =>
                      setCreateForm((prev) => ({ ...prev, name: event.target.value }))
                    }
                    required
                  />
                </div>
              </div>
            </FormContainer>
          </CardContent>
        </Card>
      ) : null}

      <ListToolbar
        searchValue={search}
        onSearchChange={setSearch}
        searchPlaceholder="Buscar por nome..."
      />

      <DataTable
        columns={columns}
        data={filteredData}
        isLoading={isLoading}
        emptyMessage="Nenhum motivo de status cadastrado."
        pagination={{
          pageIndex,
          pageSize: PAGE_SIZE,
          totalItemCount,
          onPageChange: setPageIndex,
        }}
      />
    </div>
  )
}
