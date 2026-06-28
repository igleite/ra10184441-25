'use client'

import { useParams, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { PageSkeleton } from '@/components/shared/page-skeleton'
import { PageHeader } from '@/components/shared/page-header'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiDelete, apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'

interface FeatureFlagDto {
  id: string
  name: string
  description: string
  value: boolean
  rowVersion: string
}

export default function AdminFeatureFlagDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const [featureFlag, setFeatureFlag] = useState<FeatureFlagDto | null>(null)
  const [form, setForm] = useState({ name: '', description: '', value: false })
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)
  const [showDelete, setShowDelete] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)

  useEffect(() => {
    apiGet<FeatureFlagDto>(`api/feature-flags/${params.id}`)
      .then((loaded) => {
        setFeatureFlag(loaded)
        setForm({
          name: loaded.name,
          description: loaded.description,
          value: loaded.value,
        })
      })
      .catch((err: ApiError) => {
        setError(formatApiErrorMessage(err, 'Feature flag não encontrada.'))
      })
      .finally(() => {
        setIsLoading(false)
      })
  }, [params.id])

  async function handleSubmit() {
    if (!featureFlag) {
      return
    }

    setIsSubmitting(true)
    setError(null)
    setSuccess(null)

    try {
      const updated = await apiPut<FeatureFlagDto>(`api/feature-flags/${featureFlag.id}`, {
        name: form.name,
        description: form.description,
        value: form.value,
        rowVersion: featureFlag.rowVersion,
      })
      setFeatureFlag(updated)
      setForm({
        name: updated.name,
        description: updated.description,
        value: updated.value,
      })
      setSuccess('Alterações salvas com sucesso.')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar feature flag.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!featureFlag) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/feature-flags/${featureFlag.id}?rowVersion=${encodeURIComponent(featureFlag.rowVersion)}`,
      )
      router.push('/admin/feature-flags')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir feature flag.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <PageSkeleton fields={3} />
  }

  if (!featureFlag) {
    return <FormFeedback error={error ?? 'Feature flag não encontrada.'} />
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={featureFlag.name}
        subtitle="Edição de feature flag da plataforma."
        breadcrumbs={[
          { label: 'Feature flags', href: '/admin/feature-flags' },
          { label: featureFlag.name },
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
        cancelHref="/admin/feature-flags"
        error={error}
        success={success}
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
            <Label htmlFor="description">Descrição</Label>
            <Input
              id="description"
              value={form.description}
              onChange={(event) =>
                setForm((prev) => ({ ...prev, description: event.target.value }))
              }
              required
            />
          </div>
          <div className="flex items-center gap-2">
            <input
              id="value"
              type="checkbox"
              checked={form.value}
              onChange={(event) =>
                setForm((prev) => ({ ...prev, value: event.target.checked }))
              }
              className="size-4 rounded border"
            />
            <Label htmlFor="value">Flag ativa</Label>
          </div>
        </div>
      </FormContainer>

      <ConfirmDialog
        open={showDelete}
        onOpenChange={setShowDelete}
        title="Excluir feature flag"
        description={`Confirma a exclusão de "${featureFlag.name}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
