'use client'

import Link from 'next/link'
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

interface ArtifactTypeDto {
  id: string
  name: string
  rowVersion: string
}

export default function AppArtifactTypeDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const [artifactType, setArtifactType] = useState<ArtifactTypeDto | null>(null)
  const [form, setForm] = useState({ name: '' })
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)
  const [showDelete, setShowDelete] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${params.id}`
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<ArtifactTypeDto>(`api/organizations/${organizationId}/artifact-types/${params.id}`)
      .then((loaded) => {
        if (cancelled) {
          return
        }
        setArtifactType(loaded)
        setForm({ name: loaded.name })
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }
        setError(formatApiErrorMessage(err, 'Tipo de artefato não encontrado.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, params.id])

  async function handleSubmit() {
    if (!artifactType) {
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const updated = await apiPut<ArtifactTypeDto>(
        `api/organizations/${organizationId}/artifact-types/${artifactType.id}`,
        {
          name: form.name.trim(),
          rowVersion: artifactType.rowVersion,
        },
      )
      setArtifactType(updated)
      setForm({ name: updated.name })
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar tipo de artefato.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!artifactType) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/artifact-types/${artifactType.id}?rowVersion=${encodeURIComponent(artifactType.rowVersion)}`,
      )
      router.push('/app/artifact-types')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir tipo de artefato.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <Loading fullPage label="Carregando tipo de artefato..." />
  }

  if (!artifactType) {
    return (
      <p className="text-sm text-destructive" role="alert">
        {error ?? 'Tipo de artefato não encontrado.'}
      </p>
    )
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={artifactType.name}
        subtitle="Detalhe do tipo de artefato."
        breadcrumbs={[
          { label: 'Tipos de artefato', href: '/app/artifact-types' },
          { label: artifactType.name },
        ]}
        actions={
          <div className="flex gap-2">
            <Button variant="outline" asChild>
              <Link href={`/app/artifact-types/${artifactType.id}/artifacts`}>Artefatos</Link>
            </Button>
            <Button variant="destructive" size="sm" onClick={() => setShowDelete(true)}>
              Excluir
            </Button>
          </div>
        }
      />

      {error ? (
        <p className="text-sm text-destructive" role="alert">
          {error}
        </p>
      ) : null}

      <FormContainer onSubmit={handleSubmit} isSubmitting={isSubmitting} cancelHref="/app/artifact-types">
        <div className="space-y-2">
          <Label htmlFor="name">Nome</Label>
          <Input
            id="name"
            value={form.name}
            onChange={(event) => setForm((prev) => ({ ...prev, name: event.target.value }))}
            required
          />
        </div>
      </FormContainer>

      <ConfirmDialog
        open={showDelete}
        onOpenChange={setShowDelete}
        title="Excluir tipo de artefato"
        description={`Confirma a exclusão de "${artifactType.name}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
