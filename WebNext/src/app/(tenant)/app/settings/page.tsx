'use client'

import { useEffect, useState } from 'react'

import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { PageSkeleton } from '@/components/shared/page-skeleton'
import { PageHeader } from '@/components/shared/page-header'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import { useTenant } from '@/providers/tenant-provider'

interface OrganizationDto {
  id: string
  name: string
  document: string
  slug: string
  rowVersion: string
}

export default function AppSettingsPage() {
  const { organizationId } = useTenant()
  const [organization, setOrganization] = useState<OrganizationDto | null>(null)
  const [form, setForm] = useState({ name: '', document: '', slug: '' })
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [saved, setSaved] = useState(false)

  const fetchKey = `${organizationId}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<OrganizationDto>(`api/organizations/${organizationId}`)
      .then((loaded) => {
        if (!cancelled) {
          setOrganization(loaded)
          setForm({
            name: loaded.name,
            document: loaded.document,
            slug: loaded.slug,
          })
          setError(null)
          setLoadedKey(fetchKey)
        }
      })
      .catch((err: ApiError) => {
        if (!cancelled) {
          setError(formatApiErrorMessage(err, 'Erro ao carregar organização.'))
          setLoadedKey(fetchKey)
        }
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId])

  async function handleSubmit() {
    if (!organization) {
      return
    }

    setIsSubmitting(true)
    setError(null)
    setSaved(false)

    try {
      const updated = await apiPut<OrganizationDto>(`api/organizations/${organization.id}`, {
        name: form.name.trim(),
        document: form.document.trim(),
        slug: form.slug.trim(),
        rowVersion: organization.rowVersion,
      })
      setOrganization(updated)
      setForm({
        name: updated.name,
        document: updated.document,
        slug: updated.slug,
      })
      setSaved(true)
      setReloadToken((value) => value + 1)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar configurações.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  if (isLoading) {
    return <PageSkeleton fields={3} />
  }

  if (!organization) {
    return <FormFeedback error={error ?? 'Organização não encontrada.'} />
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title="Configurações da organização"
        subtitle="Gerencie nome, documento e slug da organização."
      />

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        submitLabel="Salvar configurações"
        error={error}
        success={saved ? 'Configurações atualizadas com sucesso.' : null}
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
          <div className="space-y-2">
            <Label htmlFor="slug">Slug</Label>
            <Input
              id="slug"
              value={form.slug}
              onChange={(event) => setForm((prev) => ({ ...prev, slug: event.target.value }))}
              required
            />
          </div>
        </div>
      </FormContainer>
    </div>
  )
}
