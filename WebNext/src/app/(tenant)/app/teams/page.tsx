'use client'

import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useEffect, useMemo, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { DataTable, type ColumnDef } from '@/components/shared/data-table'
import { EmptyState } from '@/components/shared/empty-state'
import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { ListToolbar } from '@/components/shared/list-toolbar'
import { PageHeader } from '@/components/shared/page-header'
import { SelectField } from '@/components/shared/select-field'
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
import { TEAM_ROLE_OPTIONS, roleLabel } from '@/lib/role-options'
import { useTenant } from '@/providers/tenant-provider'

interface TeamDto {
  id: string
  name: string
  code: string
  roleId: string
  rowVersion: string
}

const PAGE_SIZE = 10
const ORGANIZATION_ADMIN_CODE = 'ORGANIZATION_ADMIN'

export default function AppTeamsListPage() {
  const router = useRouter()
  const { organizationId } = useTenant()
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<TeamDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${pageIndex}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({
    name: '',
    code: '',
    roleId: '',
  })
  const [deleteTarget, setDeleteTarget] = useState<TeamDto | null>(null)
  const [isDeleting, setIsDeleting] = useState(false)
  const [search, setSearch] = useState('')

  const filteredData = useMemo(
    () => filterListByQuery(data, search, ['name', 'code']),
    [data, search],
  )

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<TeamDto>>(
      `api/organizations/${organizationId}/teams?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
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
          setError(formatApiErrorMessage(err, 'Erro ao carregar times.'))
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
      const created = await apiPost<TeamDto>(
        `api/organizations/${organizationId}/teams`,
        {
          name: createForm.name.trim(),
          code: createForm.code.trim(),
          roleId: createForm.roleId.trim(),
        },
      )
      setShowCreate(false)
      setCreateForm({ name: '', code: '', roleId: '' })
      router.push(`/app/teams/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar time.'))
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
        `api/organizations/${organizationId}/teams/${deleteTarget.id}?rowVersion=${encodeURIComponent(deleteTarget.rowVersion)}`,
      )
      setDeleteTarget(null)
      setReloadToken((value) => value + 1)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir time.'))
    } finally {
      setIsDeleting(false)
    }
  }

  const columns: ColumnDef<TeamDto>[] = [
    {
      id: 'name',
      header: 'Nome',
      cell: (row) => (
        <Link href={`/app/teams/${row.id}`} className="font-medium hover:underline">
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
      id: 'roleId',
      header: 'Papel',
      cell: (row) => roleLabel(row.roleId),
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <div className="flex gap-2">
          <Button variant="outline" size="sm" asChild>
            <Link href={`/app/teams/${row.id}`}>Editar</Link>
          </Button>
          {row.code !== ORGANIZATION_ADMIN_CODE ? (
            <Button
              variant="destructive"
              size="sm"
              onClick={() => setDeleteTarget(row)}
            >
              Excluir
            </Button>
          ) : null}
        </div>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Times"
        subtitle="Estrutura de papéis por organização."
        actions={
          <Button onClick={() => setShowCreate((value) => !value)}>
            {showCreate ? 'Cancelar' : 'Novo time'}
          </Button>
        }
      />

      <FormFeedback error={!showCreate ? error : null} />

      {showCreate ? (
        <Card>
          <CardHeader>
            <CardTitle>Novo time</CardTitle>
            <CardDescription>Cadastro de time da organização.</CardDescription>
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
                <SelectField
                  id="roleId"
                  label="Papel"
                  value={createForm.roleId}
                  onChange={(value) => setCreateForm((prev) => ({ ...prev, roleId: value }))}
                  options={[...TEAM_ROLE_OPTIONS]}
                  placeholder="Selecione o papel..."
                  required
                />
              </div>
            </FormContainer>
          </CardContent>
        </Card>
      ) : null}

      {!isLoading && data.length === 0 && !showCreate ? (
        <EmptyState
          title="Nenhum time cadastrado"
          description="Crie o primeiro time para organizar papéis internos."
          action={{ label: 'Novo time', onClick: () => setShowCreate(true) }}
        />
      ) : (
        <>
          <ListToolbar
            searchValue={search}
            onSearchChange={setSearch}
            searchPlaceholder="Buscar por nome ou código..."
          />

          <DataTable
            columns={columns}
            data={filteredData}
            isLoading={isLoading}
            emptyMessage="Nenhum time cadastrado."
            pagination={{
              pageIndex,
              pageSize: PAGE_SIZE,
              totalItemCount,
              onPageChange: setPageIndex,
            }}
          />
        </>
      )}

      <ConfirmDialog
        open={Boolean(deleteTarget)}
        onOpenChange={(open) => {
          if (!open) {
            setDeleteTarget(null)
          }
        }}
        title="Excluir time"
        description={`Confirma a exclusão de "${deleteTarget?.name}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
