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
import { queryList } from '@/lib/list-query'
import { usePlatformUserLabels } from '@/hooks/use-platform-user-labels'
import { roleLabel } from '@/lib/role-options'
import { useCustomer } from '@/providers/customer-provider'
import { useTenant } from '@/providers/tenant-provider'

interface CustomerUserDto {
  id: string
  customerId: string
  userId: string
  roleId: string
  rowVersion: string
}

interface UserDto {
  id: string
  name: string
  email: string
  rowVersion: string
}

const PAGE_SIZE = 10

export default function PortalUsersListPage() {
  const router = useRouter()
  const { organizationId } = useTenant()
  const { customerId } = useCustomer()
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<CustomerUserDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({ name: '', email: '' })
  const [search, setSearch] = useState('')
  const { userName } = usePlatformUserLabels()

  const fetchKey = `${organizationId}-${customerId ?? 'none'}-${pageIndex}-${reloadToken}`
  const isLoading = customerId ? loadedKey !== fetchKey : false

  const filteredData = useMemo(
    () =>
      queryList(data, {
        search,
        searchFields: ['userId', 'roleId'],
        sortField: 'userId',
        sortDirection: 'asc',
      }),
    [data, search],
  )

  useEffect(() => {
    if (!customerId) {
      return
    }

    let cancelled = false

    void apiGet<PageDto<CustomerUserDto>>(
      `api/organizations/${organizationId}/customers/${customerId}/users?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
    )
      .then((page) => {
        if (cancelled) {
          return
        }

        const customerUsers = (page.items ?? []).filter(
          (item) => item.customerId === customerId,
        )
        setData(customerUsers)
        setTotalItemCount(customerUsers.length)
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        setError(formatApiErrorMessage(err, 'Erro ao carregar usuários do cliente.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [customerId, fetchKey, organizationId, pageIndex])

  async function handleCreate() {
    if (!customerId) {
      return
    }

    const name = createForm.name.trim()
    const email = createForm.email.trim()

    if (!name || !email) {
      setError('Informe nome e e-mail.')
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const user = await apiPost<UserDto>('api/users', { name, email })

      const created = await apiPost<CustomerUserDto>(
        `api/organizations/${organizationId}/customers/${customerId}/users`,
        {
          userId: user.id,
        },
      )
      setShowCreate(false)
      setCreateForm({ name: '', email: '' })
      setReloadToken((value) => value + 1)
      router.push(`/portal/users/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar usuário.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  const columns: ColumnDef<CustomerUserDto>[] = [
    {
      id: 'userId',
      header: 'Usuário',
      cell: (row) => (
        <Link href={`/portal/users/${row.id}`} className="font-medium hover:underline">
          {userName(row.userId)}
        </Link>
      ),
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
        <Button variant="outline" size="sm" asChild>
          <Link href={`/portal/users/${row.id}`}>Editar</Link>
        </Button>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Usuários do cliente"
        subtitle="Gerenciamento de acessos ao portal (ClientAdmin)."
        actions={
          <div className="flex gap-2">
            <Button variant="outline" onClick={() => setReloadToken((value) => value + 1)}>
              Atualizar
            </Button>
            <Button onClick={() => setShowCreate((value) => !value)}>
              {showCreate ? 'Cancelar' : 'Novo vínculo'}
            </Button>
          </div>
        }
      />

      <FormFeedback error={!showCreate ? error : null} />

      {showCreate ? (
        <Card>
          <CardHeader>
            <CardTitle>Novo vínculo de usuário</CardTitle>
            <CardDescription>Cadastre um novo usuário e vincule ao cliente.</CardDescription>
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
            </FormContainer>
          </CardContent>
        </Card>
      ) : null}

      {!customerId ? (
        <EmptyState
          title="Cliente não identificado"
          description="O token precisa de customerId para listar usuários do portal."
        />
      ) : (
        <>
          <ListToolbar
            searchValue={search}
            onSearchChange={setSearch}
            searchPlaceholder="Buscar por usuário ou papel..."
          />

          {!isLoading && filteredData.length === 0 && !search.trim() ? (
            <EmptyState
              title="Nenhum usuário vinculado"
              description="Crie o primeiro vínculo de acesso ao portal."
              action={{ label: 'Novo vínculo', onClick: () => setShowCreate(true) }}
            />
          ) : (
            <DataTable
              columns={columns}
              data={filteredData}
              isLoading={isLoading}
              emptyMessage="Nenhum vínculo de usuário cadastrado."
              pagination={{
                pageIndex,
                pageSize: PAGE_SIZE,
                totalItemCount,
                onPageChange: setPageIndex,
              }}
            />
          )}
        </>
      )}
    </div>
  )
}
