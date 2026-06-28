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
import { useTenant } from '@/providers/tenant-provider'

interface OrganizationUserDto {
  id: string
  userId: string
  teamId: string
  rowVersion: string
}

interface TeamDto {
  id: string
  name: string
}

interface UserDto {
  id: string
  name: string
  email: string
  rowVersion: string
}

const PAGE_SIZE = 10
const TEAM_PAGE_SIZE = 50

export default function AppUsersListPage() {
  const router = useRouter()
  const { organizationId } = useTenant()
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [users, setUsers] = useState<OrganizationUserDto[]>([])
  const [teams, setTeams] = useState<TeamDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({ name: '', email: '', teamId: '' })
  const [deleteTarget, setDeleteTarget] = useState<OrganizationUserDto | null>(null)
  const [isDeleting, setIsDeleting] = useState(false)
  const [search, setSearch] = useState('')

  const fetchKey = `${organizationId}-${pageIndex}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey

  const teamNameById = useMemo(() => {
    return new Map(teams.map((team) => [team.id, team.name]))
  }, [teams])

  const teamOptions = useMemo(
    () => teams.map((team) => ({ value: team.id, label: team.name })),
    [teams],
  )

  const filteredUsers = useMemo(
    () =>
      filterListByQuery(users, search, ['userId', 'teamId']).filter((row) => {
        const normalized = search.trim().toLowerCase()
        if (!normalized) {
          return true
        }
        const teamName = teamNameById.get(row.teamId) ?? ''
        return teamName.toLowerCase().includes(normalized)
      }),
    [users, search, teamNameById],
  )

  useEffect(() => {
    let cancelled = false

    async function loadData() {
      try {
        const [usersPage, teamsPage] = await Promise.all([
          apiGet<PageDto<OrganizationUserDto>>(
            `api/organizations/${organizationId}/users?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
          ).catch((err: ApiError) => {
            if (err.statusCode === 404) {
              return {
                items: [],
                totalItemCount: 0,
                pageIndex: pageIndex + 1,
                pageSize: PAGE_SIZE,
              } satisfies PageDto<OrganizationUserDto>
            }
            throw err
          }),
          apiGet<PageDto<TeamDto>>(
            `api/organizations/${organizationId}/teams?pageIndex=1&pageSize=${TEAM_PAGE_SIZE}`,
          ).catch((err: ApiError) => {
            if (err.statusCode === 404) {
              return {
                items: [],
                totalItemCount: 0,
                pageIndex: 1,
                pageSize: TEAM_PAGE_SIZE,
              } satisfies PageDto<TeamDto>
            }
            throw err
          }),
        ])

        if (!cancelled) {
          setUsers(usersPage.items ?? [])
          setTotalItemCount(usersPage.totalItemCount ?? 0)
          setTeams(teamsPage.items ?? [])
          setError(null)
          setLoadedKey(fetchKey)
        }
      } catch (err) {
        if (!cancelled) {
          const apiError = err as ApiError
          setError(formatApiErrorMessage(apiError, 'Erro ao carregar usuários da organização.'))
          setLoadedKey(fetchKey)
        }
      }
    }

    void loadData()

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, pageIndex])

  async function handleCreate() {
    const name = createForm.name.trim()
    const email = createForm.email.trim()
    const teamId = createForm.teamId.trim()

    if (!name || !email) {
      setError('Informe nome e e-mail.')
      return
    }

    if (!teamId) {
      setError('Selecione um time.')
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const user = await apiPost<UserDto>('api/users', { name, email })

      const created = await apiPost<OrganizationUserDto>(
        `api/organizations/${organizationId}/users`,
        {
          userId: user.id,
          teamId,
        },
      )
      setShowCreate(false)
      setCreateForm({ name: '', email: '', teamId: '' })
      router.push(`/app/users/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar usuário.'))
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
        `api/organizations/${organizationId}/users/${deleteTarget.id}?rowVersion=${encodeURIComponent(deleteTarget.rowVersion)}`,
      )
      setDeleteTarget(null)
      setReloadToken((value) => value + 1)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao remover usuário da organização.'))
    } finally {
      setIsDeleting(false)
    }
  }

  const columns: ColumnDef<OrganizationUserDto>[] = [
    {
      id: 'userId',
      header: 'Usuário',
      cell: (row) => (
        <Link href={`/app/users/${row.id}`} className="font-medium hover:underline">
          {row.userId}
        </Link>
      ),
    },
    {
      id: 'teamId',
      header: 'Time',
      cell: (row) => teamNameById.get(row.teamId) ?? row.teamId,
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <div className="flex gap-2">
          <Button variant="outline" size="sm" asChild>
            <Link href={`/app/users/${row.id}`}>Editar</Link>
          </Button>
          <Button variant="destructive" size="sm" onClick={() => setDeleteTarget(row)}>
            Excluir
          </Button>
        </div>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Usuários da organização"
        subtitle="Vínculos de usuários com times dentro da organização."
        actions={
          <Button onClick={() => setShowCreate((value) => !value)}>
            {showCreate ? 'Cancelar' : 'Adicionar usuário'}
          </Button>
        }
      />

      <FormFeedback error={!showCreate ? error : null} />

      {showCreate ? (
        <Card>
          <CardHeader>
            <CardTitle>Novo usuário da organização</CardTitle>
            <CardDescription>
              Cadastre um novo usuário e selecione o time para criar o vínculo.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <FormContainer
              onSubmit={handleCreate}
              isSubmitting={isSubmitting}
              submitLabel="Adicionar"
              error={error}
            >
              <div className="space-y-4">
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
                    <Label htmlFor="email">E-mail</Label>
                    <Input
                      id="email"
                      type="email"
                      value={createForm.email}
                      onChange={(event) =>
                        setCreateForm((prev) => ({ ...prev, email: event.target.value }))
                      }
                      required
                    />
                  </div>
                </div>
                <SelectField
                  id="teamId"
                  label="Time"
                  value={createForm.teamId}
                  onChange={(value) => setCreateForm((prev) => ({ ...prev, teamId: value }))}
                  options={teamOptions}
                  placeholder={
                    teams.length === 0 ? 'Nenhum time disponível' : 'Selecione o time...'
                  }
                  required
                  disabled={teams.length === 0}
                />
              </div>
            </FormContainer>
          </CardContent>
        </Card>
      ) : null}

      {users.length === 0 && !isLoading ? (
        <EmptyState
          title="Nenhum usuário vinculado"
          description="Adicione um usuário existente para operar na organização."
          action={{ label: 'Adicionar usuário', onClick: () => setShowCreate(true) }}
        />
      ) : (
        <>
          <ListToolbar
            searchValue={search}
            onSearchChange={setSearch}
            searchPlaceholder="Buscar por usuário ou time..."
          />
          <DataTable
            columns={columns}
            data={filteredUsers}
            isLoading={isLoading}
            emptyMessage="Nenhum usuário vinculado."
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
        title="Remover usuário da organização"
        description={`Confirma a remoção do usuário ${deleteTarget?.userId}?`}
        confirmLabel={isDeleting ? 'Removendo...' : 'Remover'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
