'use client'

import { useParams, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { FormContainer } from '@/components/shared/form-container'
import { Loading } from '@/components/shared/loading'
import { PageHeader } from '@/components/shared/page-header'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiDelete, apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import { useTenant } from '@/providers/tenant-provider'

interface TicketClassificationDto {
  id: string
  name: string
  code: string
  rowVersion: string
}

export default function AppTicketClassificationDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const [classification, setClassification] = useState<TicketClassificationDto | null>(null)
  const [form, setForm] = useState({ name: '', code: '' })
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)
  const [showDelete, setShowDelete] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${params.id}`
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<TicketClassificationDto>(
      `api/organizations/${organizationId}/ticket-classifications/${params.id}`,
    )
      .then((loaded) => {
        if (cancelled) {
          return
        }
        setClassification(loaded)
        setForm({ name: loaded.name, code: loaded.code })
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }
        setError(formatApiErrorMessage(err, 'Classificação não encontrada.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, params.id])

  async function handleSubmit() {
    if (!classification) {
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const updated = await apiPut<TicketClassificationDto>(
        `api/organizations/${organizationId}/ticket-classifications/${classification.id}`,
        {
          name: form.name.trim(),
          code: form.code.trim(),
          rowVersion: classification.rowVersion,
        },
      )
      setClassification(updated)
      setForm({ name: updated.name, code: updated.code })
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar classificação.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!classification) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/ticket-classifications/${classification.id}?rowVersion=${encodeURIComponent(classification.rowVersion)}`,
      )
      router.push('/app/ticket-classifications')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir classificação.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <Loading fullPage label="Carregando classificação..." />
  }

  if (!classification) {
    return (
      <p className="text-sm text-destructive" role="alert">
        {error ?? 'Classificação não encontrada.'}
      </p>
    )
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={classification.name}
        subtitle="Detalhe da classificação de ticket."
        breadcrumbs={[
          { label: 'Classificações', href: '/app/ticket-classifications' },
          { label: classification.name },
        ]}
        actions={
          <Button variant="destructive" size="sm" onClick={() => setShowDelete(true)}>
            Excluir
          </Button>
        }
      />

      {error ? (
        <p className="text-sm text-destructive" role="alert">
          {error}
        </p>
      ) : null}

      <FormContainer onSubmit={handleSubmit} isSubmitting={isSubmitting} cancelHref="/app/ticket-classifications">
        <div className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="name">Nome</Label>
            <Input
              id="name"
              value={form.name}
              onChange={(event) => setForm((prev) => ({ ...prev, name: event.target.value }))}
              required
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="code">Código</Label>
            <Input
              id="code"
              value={form.code}
              onChange={(event) => setForm((prev) => ({ ...prev, code: event.target.value }))}
              required
            />
          </div>
        </div>
      </FormContainer>

      <ConfirmDialog
        open={showDelete}
        onOpenChange={setShowDelete}
        title="Excluir classificação"
        description={`Confirma a exclusão de "${classification.name}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
