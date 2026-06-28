'use client'

import Link from 'next/link'
import { useRouter } from 'next/navigation'
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

interface ArtifactTypeDto {
  id: string
  name: string
}

const PAGE_SIZE = 10

export default function AppArtifactTypesPage() {
  const router = useRouter()
  const { organizationId } = useTenant()
  const [pageIndex, setPageIndex] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<ArtifactTypeDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({ name: '' })
  const [search, setSearch] = useState('')
  const fetchKey = `${organizationId}-${pageIndex}`

  const filteredData = useMemo(
    () => filterListByQuery(data, search, ['name']),
    [data, search],
  )
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<ArtifactTypeDto>>(
      `api/organizations/${organizationId}/artifact-types?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
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

        setError(formatApiErrorMessage(err, 'Erro ao carregar tipos de artefato.'))
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
      const created = await apiPost<ArtifactTypeDto>(`api/organizations/${organizationId}/artifact-types`, {
        name: createForm.name.trim(),
      })
      setShowCreate(false)
      setCreateForm({ name: '' })
      router.push(`/app/artifact-types/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar tipo de artefato.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  const columns: ColumnDef<ArtifactTypeDto>[] = [
    {
      id: 'name',
      header: 'Nome',
      cell: (row) => (
        <Link href={`/app/artifact-types/${row.id}`} className="font-medium hover:underline">
          {row.name}
        </Link>
      ),
    },
    {
      id: 'artifacts',
      header: 'Artefatos',
      cell: (row) => (
        <Link
          href={`/app/artifact-types/${row.id}/artifacts`}
          className="text-primary hover:underline"
        >
          Ver artefatos
        </Link>
      ),
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <Button variant="outline" size="sm" asChild>
          <Link href={`/app/artifact-types/${row.id}`}>Detalhes</Link>
        </Button>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Tipos de artefato"
        subtitle="Estruturas usadas para organização dos artefatos."
        actions={
          <Button onClick={() => setShowCreate((value) => !value)}>
            {showCreate ? 'Cancelar' : 'Novo tipo'}
          </Button>
        }
      />

      <FormFeedback error={!showCreate ? error : null} />

      {showCreate ? (
        <Card>
          <CardHeader>
            <CardTitle>Novo tipo de artefato</CardTitle>
            <CardDescription>Cadastro de tipo para agrupamento de artefatos.</CardDescription>
          </CardHeader>
          <CardContent>
            <FormContainer
              onSubmit={handleCreate}
              isSubmitting={isSubmitting}
              submitLabel="Criar"
              error={error}
            >
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
        emptyMessage="Nenhum tipo de artefato cadastrado."
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
