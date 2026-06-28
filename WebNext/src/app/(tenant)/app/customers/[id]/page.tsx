'use client'

import Link from 'next/link'
import { useParams, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { FormContainer } from '@/components/shared/form-container'
import { Loading } from '@/components/shared/loading'
import { PageHeader } from '@/components/shared/page-header'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiDelete, apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import { PermissionGate } from '@/components/shared/permission-gate'
import { usePermissions } from '@/hooks/use-permissions'
import { useTenant } from '@/providers/tenant-provider'

interface CustomerDto {
  id: string
  name: string
  document: string
  rowVersion: string
}

export default function AppCustomerDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { can } = usePermissions()
  const { organizationId } = useTenant()
  const [customer, setCustomer] = useState<CustomerDto | null>(null)
  const [form, setForm] = useState({ name: '', document: '' })
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [showDelete, setShowDelete] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)

  useEffect(() => {
    apiGet<CustomerDto>(`api/organizations/${organizationId}/customers/${params.id}`)
      .then((loaded) => {
        setCustomer(loaded)
        setForm({ name: loaded.name, document: loaded.document })
      })
      .catch((err: ApiError) => {
        setError(formatApiErrorMessage(err, 'Cliente não encontrado.'))
      })
      .finally(() => {
        setIsLoading(false)
      })
  }, [organizationId, params.id])

  const canManage = can('customers:manage')

  async function handleSubmit() {
    if (!customer || !canManage) {
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const updated = await apiPut<CustomerDto>(
        `api/organizations/${organizationId}/customers/${customer.id}`,
        {
          name: form.name.trim(),
          document: form.document.trim(),
          rowVersion: customer.rowVersion,
        },
      )
      setCustomer(updated)
      setForm({ name: updated.name, document: updated.document })
      router.push('/app/customers')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar cliente.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!customer || !canManage) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/customers/${customer.id}?rowVersion=${encodeURIComponent(customer.rowVersion)}`,
      )
      router.push('/app/customers')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir cliente.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <Loading fullPage label="Carregando cliente..." />
  }

  if (!customer) {
    return (
      <p className="text-sm text-destructive" role="alert">
        {error ?? 'Cliente não encontrado.'}
      </p>
    )
  }

  return (
    <div className="mx-auto max-w-3xl space-y-8">
      <PageHeader
        title={customer.name}
        subtitle="Hub de operações do cliente."
        breadcrumbs={[
          { label: 'Clientes', href: '/app/customers' },
          { label: customer.name },
        ]}
        actions={
          <PermissionGate action="customers:manage">
            <Button variant="destructive" size="sm" onClick={() => setShowDelete(true)}>
              Excluir
            </Button>
          </PermissionGate>
        }
      />

      {error ? (
        <p className="text-sm text-destructive" role="alert">
          {error}
        </p>
      ) : null}

      <Card>
        <CardHeader>
          <CardTitle>Sub-recursos</CardTitle>
        </CardHeader>
        <CardContent className="flex flex-wrap gap-2">
          <PermissionGate action="customers:manage">
            <Button variant="outline" asChild>
              <Link href={`/app/customers/${customer.id}/users`}>Usuários</Link>
            </Button>
            <Button variant="outline" asChild>
              <Link href={`/app/customers/${customer.id}/artifacts`}>Artefatos</Link>
            </Button>
          </PermissionGate>
          <Button variant="outline" asChild>
            <Link href={`/app/tickets?customerId=${customer.id}`}>Tickets</Link>
          </Button>
        </CardContent>
      </Card>

      {canManage ? (
        <FormContainer
          onSubmit={handleSubmit}
          isSubmitting={isSubmitting}
          cancelHref="/app/customers"
        >
          <div className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="name">Nome</Label>
              <Input
                id="name"
                value={form.name}
                onChange={(event) =>
                  setForm((prev) => ({ ...prev, name: event.target.value }))
                }
                required
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="document">Documento</Label>
              <Input
                id="document"
                value={form.document}
                onChange={(event) =>
                  setForm((prev) => ({ ...prev, document: event.target.value }))
                }
                required
              />
            </div>
          </div>
        </FormContainer>
      ) : (
        <Card>
          <CardHeader>
            <CardTitle>Dados do cliente</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="name">Nome</Label>
              <Input id="name" value={form.name} readOnly disabled />
            </div>
            <div className="space-y-2">
              <Label htmlFor="document">Documento</Label>
              <Input id="document" value={form.document} readOnly disabled />
            </div>
          </CardContent>
        </Card>
      )}

      {canManage ? (
        <ConfirmDialog
          open={showDelete}
          onOpenChange={setShowDelete}
          title="Excluir cliente"
          description={`Confirma a exclusão de "${customer.name}"?`}
          confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
          variant="destructive"
          onConfirm={handleDelete}
        />
      ) : null}
    </div>
  )
}
