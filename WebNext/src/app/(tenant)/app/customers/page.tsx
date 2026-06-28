'use client'

import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useEffect, useMemo, useState } from 'react'

import { DataTable, type ColumnDef } from '@/components/shared/data-table'
import { EmptyState } from '@/components/shared/empty-state'
import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { ListToolbar } from '@/components/shared/list-toolbar'
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
import { PermissionGate } from '@/components/shared/permission-gate'
import { usePermissions } from '@/hooks/use-permissions'
import { useTenant } from '@/providers/tenant-provider'

interface CustomerDto {
  id: string
  name: string
  document: string
  rowVersion: string
}

const PAGE_SIZE = 10

export default function AppCustomersListPage() {
  const router = useRouter()
  const { can } = usePermissions()
  const { organizationId } = useTenant()
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<CustomerDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${pageIndex}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({
    name: '',
    document: '',
  })
  const [search, setSearch] = useState('')

  const filteredData = useMemo(
    () => filterListByQuery(data, search, ['name', 'document']),
    [data, search],
  )

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<CustomerDto>>(
      `api/organizations/${organizationId}/customers?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
    )
      .then((page) => {
        if (!cancelled) {
          setData(page.items ?? [])
          setTotalItemCount(page.totalItemCount)
          setError(null)
          setLoadedKey(fetchKey)
        }
      })
      .catch((err: ApiError) => {
        if (!cancelled) {
          setError(formatApiErrorMessage(err, 'Erro ao carregar clientes.'))
          setLoadedKey(fetchKey)
        }
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, pageIndex])

  async function handleCreate() {
    setIsSubmitting(true)
    setError(null)

    try {
      const created = await apiPost<CustomerDto>(
        `api/organizations/${organizationId}/customers`,
        {
          name: createForm.name.trim(),
          document: createForm.document.trim(),
        },
      )
      setShowCreate(false)
      setCreateForm({ name: '', document: '' })
      setReloadToken((value) => value + 1)
      router.push(`/app/customers/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar cliente.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  const columns: ColumnDef<CustomerDto>[] = [
    {
      id: 'name',
      header: 'Nome',
      cell: (row) => (
        <Link href={`/app/customers/${row.id}`} className="font-medium hover:underline">
          {row.name}
        </Link>
      ),
    },
    {
      id: 'document',
      header: 'Documento',
      cell: (row) => row.document,
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <Button variant="outline" size="sm" asChild>
          <Link href={`/app/customers/${row.id}`}>Abrir</Link>
        </Button>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Clientes"
        subtitle="Base de clientes da organização."
        actions={
          <PermissionGate action="customers:manage">
            <Button onClick={() => setShowCreate((value) => !value)}>
              {showCreate ? 'Cancelar' : 'Novo cliente'}
            </Button>
          </PermissionGate>
        }
      />

      <FormFeedback error={!showCreate ? error : null} />

      {showCreate ? (
        <Card>
          <CardHeader>
            <CardTitle>Novo cliente</CardTitle>
            <CardDescription>Cadastro de cliente para portal e tickets.</CardDescription>
          </CardHeader>
          <CardContent>
            <FormContainer
              onSubmit={handleCreate}
              isSubmitting={isSubmitting}
              submitLabel="Criar"
              error={error}
            >
              <div className="grid gap-4 sm:grid-cols-2">
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
                <div className="space-y-2">
                  <Label htmlFor="document">Documento</Label>
                  <Input
                    id="document"
                    value={createForm.document}
                    onChange={(event) =>
                      setCreateForm((prev) => ({ ...prev, document: event.target.value }))
                    }
                    required
                  />
                </div>
              </div>
            </FormContainer>
          </CardContent>
        </Card>
      ) : null}

      {data.length === 0 && !isLoading ? (
        <EmptyState
          title="Nenhum cliente cadastrado"
          description="Cadastre clientes para vincular usuários e tickets."
          action={
            can('customers:manage')
              ? { label: 'Novo cliente', onClick: () => setShowCreate(true) }
              : undefined
          }
        />
      ) : (
        <>
          <ListToolbar
            searchValue={search}
            onSearchChange={setSearch}
            searchPlaceholder="Buscar por nome ou documento..."
          />
          <DataTable
            columns={columns}
            data={filteredData}
            isLoading={isLoading}
            emptyMessage="Nenhum cliente cadastrado."
            pagination={{
              pageIndex,
              pageSize: PAGE_SIZE,
              totalItemCount,
              onPageChange: setPageIndex,
            }}
          />
        </>
      )}
    </div>
  )
}
