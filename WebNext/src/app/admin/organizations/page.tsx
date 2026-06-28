'use client'

import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useEffect, useMemo, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
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
import { apiDelete, apiGet, apiPost } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { filterListByQuery } from '@/lib/filter-list'
import { buildTenantUrl } from '@/lib/tenant'

interface OrganizationDto {
  id: string
  name: string
  document: string
  slug: string
  rowVersion: string
}

const PAGE_SIZE = 10

export default function AdminOrganizationsListPage() {
  const router = useRouter()
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<OrganizationDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const fetchKey = `${pageIndex}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({
    name: '',
    document: '',
    slug: '',
  })
  const [deleteTarget, setDeleteTarget] = useState<OrganizationDto | null>(null)
  const [isDeleting, setIsDeleting] = useState(false)
  const [search, setSearch] = useState('')

  const filteredData = useMemo(
    () => filterListByQuery(data, search, ['name', 'slug', 'document']),
    [data, search],
  )

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<OrganizationDto>>(
      `api/organizations?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
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
          setError(formatApiErrorMessage(err, 'Erro ao carregar organizações.'))
          setLoadedKey(fetchKey)
        }
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, pageIndex])

  async function handleCreate() {
    setIsSubmitting(true)
    setError(null)

    try {
      const created = await apiPost<OrganizationDto>('api/organizations', createForm)
      setShowCreate(false)
      setCreateForm({ name: '', document: '', slug: '' })
      router.push(`/admin/organizations/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar organização.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!deleteTarget) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${deleteTarget.id}?rowVersion=${encodeURIComponent(deleteTarget.rowVersion)}`,
      )
      setDeleteTarget(null)
      setReloadToken((value) => value + 1)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir organização.'))
    } finally {
      setIsDeleting(false)
    }
  }

  const columns: ColumnDef<OrganizationDto>[] = [
    {
      id: 'name',
      header: 'Nome',
      cell: (row) => (
        <Link
          href={`/admin/organizations/${row.id}`}
          className="font-medium hover:underline"
        >
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
      id: 'slug',
      header: 'Subdomínio',
      cell: (row) => (
        <a
          href={buildTenantUrl(row.slug, '/app')}
          className="text-primary hover:underline"
          target="_blank"
          rel="noreferrer"
        >
          {row.slug}
        </a>
      ),
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <div className="flex gap-2">
          <Button variant="outline" size="sm" asChild>
            <Link href={`/admin/organizations/${row.id}`}>Editar</Link>
          </Button>
          <Button
            variant="destructive"
            size="sm"
            onClick={() => setDeleteTarget(row)}
          >
            Excluir
          </Button>
        </div>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Organizações"
        subtitle="Todas as organizações cadastradas na plataforma."
        actions={
          <Button onClick={() => setShowCreate((value) => !value)}>
            {showCreate ? 'Cancelar' : 'Nova organização'}
          </Button>
        }
      />

      <FormFeedback error={!showCreate ? error : null} />

      {showCreate ? (
        <Card>
          <CardHeader>
            <CardTitle>Nova organização</CardTitle>
            <CardDescription>Cadastro de organização na plataforma.</CardDescription>
          </CardHeader>
          <CardContent>
            <FormContainer
              onSubmit={handleCreate}
              isSubmitting={isSubmitting}
              submitLabel="Criar"
              error={error}
            >
              <div className="grid gap-4 sm:grid-cols-3">
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
                      setCreateForm((prev) => ({
                        ...prev,
                        document: event.target.value,
                      }))
                    }
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="slug">Slug</Label>
                  <Input
                    id="slug"
                    value={createForm.slug}
                    onChange={(event) =>
                      setCreateForm((prev) => ({ ...prev, slug: event.target.value }))
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
        searchPlaceholder="Buscar por nome, slug ou documento..."
      />

      <DataTable
        columns={columns}
        data={filteredData}
        isLoading={isLoading}
        emptyMessage="Nenhuma organização cadastrada."
        pagination={{
          pageIndex,
          pageSize: PAGE_SIZE,
          totalItemCount,
          onPageChange: setPageIndex,
        }}
      />

      <ConfirmDialog
        open={Boolean(deleteTarget)}
        onOpenChange={(open) => {
          if (!open) {
            setDeleteTarget(null)
          }
        }}
        title="Excluir organização"
        description={`Confirma a exclusão de "${deleteTarget?.name}"? Esta ação não pode ser desfeita.`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
