'use client'

import { useParams, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { PageHeader } from '@/components/shared/page-header'
import { PageSkeleton } from '@/components/shared/page-skeleton'
import { PlatformUserPicker } from '@/components/shared/platform-user-picker'
import { Button } from '@/components/ui/button'
import { apiDelete, apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import { useTenant } from '@/providers/tenant-provider'

interface CustomerUserDto {
  id: string
  customerId: string
  userId: string
  roleId: string
  rowVersion: string
}

export default function AppCustomerUserDetailPage() {
  const params = useParams<{ id: string; userId: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const [membership, setMembership] = useState<CustomerUserDto | null>(null)
  const [form, setForm] = useState({ userId: '', userLabel: '' })
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [showDelete, setShowDelete] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)

  useEffect(() => {
    apiGet<CustomerUserDto>(
      `api/organizations/${organizationId}/customers/${params.id}/users/${params.userId}`,
    )
      .then((loaded) => {
        setMembership(loaded)
        setForm({ userId: loaded.userId, userLabel: loaded.userId })
      })
      .catch((err: ApiError) => {
        setError(formatApiErrorMessage(err, 'Vínculo não encontrado.'))
      })
      .finally(() => {
        setIsLoading(false)
      })
  }, [organizationId, params.id, params.userId])

  async function handleSubmit() {
    if (!membership) {
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const updated = await apiPut<CustomerUserDto>(
        `api/organizations/${organizationId}/customers/${params.id}/users/${params.userId}`,
        {
          userId: form.userId.trim(),
          rowVersion: membership.rowVersion,
        },
      )
      setMembership(updated)
      setForm({ userId: updated.userId, userLabel: form.userLabel || updated.userId })
      router.push(`/app/customers/${params.id}/users`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar vínculo.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!membership) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/customers/${params.id}/users/${params.userId}?rowVersion=${encodeURIComponent(membership.rowVersion)}`,
      )
      router.push(`/app/customers/${params.id}/users`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir vínculo.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <PageSkeleton fields={1} />
  }

  if (!membership) {
    return <FormFeedback error={error ?? 'Vínculo não encontrado.'} />
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={`Vínculo ${membership.id}`}
        subtitle="Edição de vínculo do usuário com o cliente."
        breadcrumbs={[
          { label: 'Clientes', href: '/app/customers' },
          { label: 'Detalhe', href: `/app/customers/${params.id}` },
          { label: 'Usuários', href: `/app/customers/${params.id}/users` },
          { label: membership.id },
        ]}
        actions={
          <Button variant="destructive" size="sm" onClick={() => setShowDelete(true)}>
            Excluir
          </Button>
        }
      />

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        cancelHref={`/app/customers/${params.id}/users`}
        error={error}
      >
        <PlatformUserPicker
          value={form.userId}
          displayLabel={form.userLabel}
          onChange={(userId, label) => setForm({ userId, userLabel: label })}
          required
        />
      </FormContainer>

      <ConfirmDialog
        open={showDelete}
        onOpenChange={setShowDelete}
        title="Excluir vínculo"
        description={`Confirma a exclusão do vínculo "${membership.id}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
