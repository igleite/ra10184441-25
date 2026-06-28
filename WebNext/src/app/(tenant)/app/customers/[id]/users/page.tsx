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
import { roleLabel } from '@/lib/role-options'
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

export default function AppCustomerUsersListPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<CustomerUserDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${params.id}-${pageIndex}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({ name: '', email: '' })
  const [search, setSearch] = useState('')

  const filteredData = useMemo(
    () => filterListByQuery(data, search, ['userId', 'roleId']),
    [data, search],
  )

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<CustomerUserDto>>(
      `api/organizations/${organizationId}/customers/${params.id}/users?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
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
          setError(formatApiErrorMessage(err, 'Erro ao carregar usuários do cliente.'))
          setLoadedKey(fetchKey)
        }
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, pageIndex, params.id])

  async function handleCreate() {
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
        `api/organizations/${organizationId}/customers/${params.id}/users`,
        {
          userId: user.id,
        },
      )
      setShowCreate(false)
      setCreateForm({ name: '', email: '' })
      setReloadToken((value) => value + 1)
      router.push(`/app/customers/${params.id}/users/${created.id}`)
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
        <Link
          href={`/app/customers/${params.id}/users/${row.id}`}
          className="font-medium hover:underline"
        >
          {row.userId}
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
          <Link href={`/app/customers/${params.id}/users/${row.id}`}>Editar</Link>
        </Button>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Usuários do cliente"
        subtitle="Vínculos de usuários com acesso ao portal."
        breadcrumbs={[
          { label: 'Clientes', href: '/app/customers' },
          { label: 'Detalhe', href: `/app/customers/${params.id}` },
          { label: 'Usuários' },
        ]}
        actions={
          <Button onClick={() => setShowCreate((value) => !value)}>
            {showCreate ? 'Cancelar' : 'Novo vínculo'}
          </Button>
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

      <ListToolbar
        searchValue={search}
        onSearchChange={setSearch}
        searchPlaceholder="Buscar por usuário ou papel..."
      />

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
    </div>
  )
}
