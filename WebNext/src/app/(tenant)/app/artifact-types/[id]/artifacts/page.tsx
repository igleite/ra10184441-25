'use client'

import Link from 'next/link'
import { useParams, useRouter } from 'next/navigation'
import { useEffect, useMemo, useState } from 'react'

import { DataTable, type ColumnDef } from '@/components/shared/data-table'
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
import { useTenant } from '@/providers/tenant-provider'

interface ArtifactDto {
  id: string
  name: string
  code: string
}

const PAGE_SIZE = 10

export default function AppArtifactsByTypeListPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const [pageIndex, setPageIndex] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<ArtifactDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({ name: '', code: '' })
  const [search, setSearch] = useState('')
  const fetchKey = `${organizationId}-${params.id}-${pageIndex}`

  const filteredData = useMemo(
    () => filterListByQuery(data, search, ['name', 'code']),
    [data, search],
  )
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<ArtifactDto>>(
      `api/organizations/${organizationId}/artifact-types/${params.id}/artifacts?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
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

        setError(formatApiErrorMessage(err, 'Erro ao carregar artefatos.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, pageIndex, params.id])

  async function handleCreate() {
    setIsSubmitting(true)
    setError(null)

    try {
      const created = await apiPost<ArtifactDto>(
        `api/organizations/${organizationId}/artifact-types/${params.id}/artifacts`,
        {
          name: createForm.name.trim(),
          code: createForm.code.trim(),
        },
      )
      setShowCreate(false)
      setCreateForm({ name: '', code: '' })
      router.push(`/app/artifact-types/${params.id}/artifacts/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar artefato.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  const columns: ColumnDef<ArtifactDto>[] = [
    {
      id: 'name',
      header: 'Nome',
      cell: (row) => (
        <Link
          href={`/app/artifact-types/${params.id}/artifacts/${row.id}`}
          className="font-medium hover:underline"
        >
          {row.name}
        </Link>
      ),
    },
    {
      id: 'code',
      header: 'Código',
      cell: (row) => row.code,
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <Button variant="outline" size="sm" asChild>
          <Link href={`/app/artifact-types/${params.id}/artifacts/${row.id}`}>Detalhes</Link>
        </Button>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Artefatos por tipo"
        subtitle="Artefatos vinculados ao tipo selecionado."
        breadcrumbs={[
          { label: 'Tipos de artefato', href: '/app/artifact-types' },
          { label: 'Artefatos' },
        ]}
        actions={
          <Button onClick={() => setShowCreate((value) => !value)}>
            {showCreate ? 'Cancelar' : 'Novo artefato'}
          </Button>
        }
      />

      <FormFeedback error={!showCreate ? error : null} />

      {showCreate ? (
        <Card>
          <CardHeader>
            <CardTitle>Novo artefato</CardTitle>
            <CardDescription>Cadastro de artefato para o tipo atual.</CardDescription>
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
                  <Label htmlFor="code">Código</Label>
                  <Input
                    id="code"
                    value={createForm.code}
                    onChange={(event) =>
                      setCreateForm((prev) => ({ ...prev, code: event.target.value }))
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
        searchPlaceholder="Buscar por nome ou código..."
      />

      <DataTable
        columns={columns}
        data={filteredData}
        isLoading={isLoading}
        emptyMessage="Nenhum artefato cadastrado para este tipo."
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
