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

interface ArtifactDto {
  id: string
  artifactTypeId: string
  name: string
  code: string
  rowVersion: string
}

export default function AppArtifactByTypeDetailPage() {
  const params = useParams<{ id: string; artifactId: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const [artifact, setArtifact] = useState<ArtifactDto | null>(null)
  const [form, setForm] = useState({ name: '', code: '' })
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)
  const [showDelete, setShowDelete] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${params.id}-${params.artifactId}`
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<ArtifactDto>(
      `api/organizations/${organizationId}/artifact-types/${params.id}/artifacts/${params.artifactId}`,
    )
      .then((loaded) => {
        if (cancelled) {
          return
        }
        setArtifact(loaded)
        setForm({ name: loaded.name, code: loaded.code })
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }
        setError(formatApiErrorMessage(err, 'Artefato não encontrado.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, params.artifactId, params.id])

  async function handleSubmit() {
    if (!artifact) {
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const updated = await apiPut<ArtifactDto>(
        `api/organizations/${organizationId}/artifact-types/${params.id}/artifacts/${artifact.id}`,
        {
          artifactTypeId: params.id,
          name: form.name.trim(),
          code: form.code.trim(),
          rowVersion: artifact.rowVersion,
        },
      )
      setArtifact(updated)
      setForm({ name: updated.name, code: updated.code })
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar artefato.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!artifact) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/artifact-types/${params.id}/artifacts/${artifact.id}?rowVersion=${encodeURIComponent(artifact.rowVersion)}`,
      )
      router.push(`/app/artifact-types/${params.id}/artifacts`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir artefato.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <Loading fullPage label="Carregando artefato..." />
  }

  if (!artifact) {
    return (
      <p className="text-sm text-destructive" role="alert">
        {error ?? 'Artefato não encontrado.'}
      </p>
    )
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={artifact.name}
        subtitle="Detalhe do artefato."
        breadcrumbs={[
          { label: 'Tipos de artefato', href: '/app/artifact-types' },
          { label: 'Artefatos', href: `/app/artifact-types/${params.id}/artifacts` },
          { label: artifact.name },
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

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        cancelHref={`/app/artifact-types/${params.id}/artifacts`}
      >
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
        title="Excluir artefato"
        description={`Confirma a exclusão de "${artifact.name}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
