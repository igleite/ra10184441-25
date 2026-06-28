'use client'

import { useParams, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { EmptyState } from '@/components/shared/empty-state'
import { FormContainer } from '@/components/shared/form-container'
import { PageHeader } from '@/components/shared/page-header'
import { PageSkeleton } from '@/components/shared/page-skeleton'
import { PlatformUserPicker } from '@/components/shared/platform-user-picker'
import { Button } from '@/components/ui/button'
import { apiDelete, apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import { usePlatformUserLabels } from '@/hooks/use-platform-user-labels'
import { useCustomer } from '@/providers/customer-provider'
import { useTenant } from '@/providers/tenant-provider'

interface CustomerUserDto {
  id: string
  customerId: string
  userId: string
  roleId: string
  rowVersion: string
}

export default function PortalUserDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const { customerId } = useCustomer()
  const { userName } = usePlatformUserLabels()
  const [membership, setMembership] = useState<CustomerUserDto | null>(null)
  const [form, setForm] = useState({ userId: '', userLabel: '' })
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [showDelete, setShowDelete] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)

  const fetchKey = `${organizationId}-${customerId ?? 'none'}-${params.id}`
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    if (!customerId) {
      return
    }

    let cancelled = false

    void apiGet<CustomerUserDto>(
      `api/organizations/${organizationId}/customers/${customerId}/users/${params.id}`,
    )
      .then((loaded) => {
        if (cancelled) {
          return
        }

        if (loaded.customerId !== customerId) {
          setMembership(null)
          setError('Vínculo não encontrado.')
          setLoadedKey(fetchKey)
          return
        }

        setMembership(loaded)
        setForm({ userId: loaded.userId, userLabel: loaded.userId })
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        setError(formatApiErrorMessage(err, 'Vínculo não encontrado.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [customerId, fetchKey, organizationId, params.id])

  async function handleSubmit() {
    if (!membership || !customerId) {
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const updated = await apiPut<CustomerUserDto>(
        `api/organizations/${organizationId}/customers/${customerId}/users/${params.id}`,
        {
          userId: form.userId.trim(),
          rowVersion: membership.rowVersion,
        },
      )
      setMembership(updated)
      setForm({ userId: updated.userId, userLabel: form.userLabel || updated.userId })
      router.push('/portal/users')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar vínculo.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!membership || !customerId) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/customers/${customerId}/users/${params.id}?rowVersion=${encodeURIComponent(membership.rowVersion)}`,
      )
      router.push('/portal/users')
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
    return (
      <EmptyState
        title="Vínculo não encontrado"
        description={error ?? 'Não foi possível carregar o usuário do portal.'}
        action={{ label: 'Voltar', href: '/portal/users' }}
      />
    )
  }

  const displayName = form.userLabel || userName(membership.userId)

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={displayName}
        subtitle="Edição de usuário do portal do cliente."
        breadcrumbs={[
          { label: 'Usuários', href: '/portal/users' },
          { label: displayName },
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
        cancelHref="/portal/users"
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
        description={`Confirma a exclusão do vínculo de "${displayName}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
