'use client'

import { useParams, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { SelectField } from '@/components/shared/select-field'
import { EmptyState } from '@/components/shared/empty-state'
import { FormContainer } from '@/components/shared/form-container'
import { Loading } from '@/components/shared/loading'
import { PageHeader } from '@/components/shared/page-header'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiDelete, apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import {
  STATUS_TYPE_OPTIONS,
  statusTypeToFormValue,
  type StatusTypeField,
} from '@/lib/status-type'
import { useTenant } from '@/providers/tenant-provider'

interface StatusReasonDto {
  id: string
  type: StatusTypeField
  name: string
  rowVersion: string
}

export default function AppTicketStatusReasonDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const [statusReason, setStatusReason] = useState<StatusReasonDto | null>(null)
  const [form, setForm] = useState({ type: '1', name: '' })
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)
  const [showDelete, setShowDelete] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [saved, setSaved] = useState(false)
  const fetchKey = `${organizationId}-${params.id}`
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<StatusReasonDto>(`api/organizations/${organizationId}/ticket-status-reasons/${params.id}`)
      .then((loaded) => {
        if (cancelled) {
          return
        }
        setStatusReason(loaded)
        setForm({ type: statusTypeToFormValue(loaded.type), name: loaded.name })
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }
        setError(formatApiErrorMessage(err, 'Motivo de status não encontrado.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, params.id])

  async function handleSubmit() {
    if (!statusReason) {
      return
    }

    setIsSubmitting(true)
    setError(null)
    setSaved(false)

    try {
      const updated = await apiPut<StatusReasonDto>(
        `api/organizations/${organizationId}/ticket-status-reasons/${statusReason.id}`,
        {
          type: Number(form.type),
          name: form.name.trim(),
          rowVersion: statusReason.rowVersion,
        },
      )
      setStatusReason(updated)
      setForm({ type: statusTypeToFormValue(updated.type), name: updated.name })
      setSaved(true)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar motivo de status.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!statusReason) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/ticket-status-reasons/${statusReason.id}?rowVersion=${encodeURIComponent(statusReason.rowVersion)}`,
      )
      router.push('/app/ticket-status-reasons')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir motivo de status.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <Loading fullPage label="Carregando motivo de status..." />
  }

  if (!statusReason) {
    return (
      <EmptyState
        title="Motivo não encontrado"
        description={error ?? 'Não foi possível carregar o motivo de status.'}
        action={{ label: 'Voltar', href: '/app/ticket-status-reasons' }}
      />
    )
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={statusReason.name}
        subtitle="Detalhe do motivo de status."
        breadcrumbs={[
          { label: 'Motivos de status', href: '/app/ticket-status-reasons' },
          { label: statusReason.name },
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
        cancelHref="/app/ticket-status-reasons"
        error={error}
        success={saved ? 'Alterações salvas com sucesso.' : null}
      >
        <div className="space-y-4">
          <SelectField
            id="type"
            label="Tipo"
            value={form.type}
            onChange={(value) => setForm((prev) => ({ ...prev, type: value }))}
            options={[...STATUS_TYPE_OPTIONS]}
            placeholder="Selecione o tipo"
            required
          />
          <div className="space-y-2">
            <Label htmlFor="name">Nome</Label>
            <Input
              id="name"
              value={form.name}
              onChange={(event) => setForm((prev) => ({ ...prev, name: event.target.value }))}
              required
            />
          </div>
        </div>
      </FormContainer>

      <ConfirmDialog
        open={showDelete}
        onOpenChange={setShowDelete}
        title="Excluir motivo de status"
        description={`Confirma a exclusão de "${statusReason.name}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
