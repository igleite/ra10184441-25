'use client'

import { useParams, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { PageSkeleton } from '@/components/shared/page-skeleton'
import { PageHeader } from '@/components/shared/page-header'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { isUserOrganizationMember } from '@/lib/account-organizations'
import { apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import { buildTenantUrl } from '@/lib/tenant'
import { useAuth } from '@/providers/auth-provider'

interface OrganizationDto {
  id: string
  name: string
  document: string
  slug: string
  rowVersion: string
}

export default function AccountOrganizationDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { user } = useAuth()
  const [organization, setOrganization] = useState<OrganizationDto | null>(null)
  const [form, setForm] = useState({ name: '', document: '', slug: '' })
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [saved, setSaved] = useState(false)

  useEffect(() => {
    if (!user?.userId) {
      return
    }

    let cancelled = false

    void apiGet<OrganizationDto>(`api/organizations/${params.id}`)
      .then(async (org) => {
        if (cancelled) {
          return
        }

        const isMember = await isUserOrganizationMember(org.id, user.userId)
        if (!isMember) {
          router.replace('/403')
          return
        }

        setOrganization(org)
        setForm({ name: org.name, document: org.document, slug: org.slug })
      })
      .catch((err: ApiError) => {
        if (!cancelled) {
          setError(formatApiErrorMessage(err, 'Organização não encontrada.'))
        }
      })
      .finally(() => {
        if (!cancelled) {
          setIsLoading(false)
        }
      })

    return () => {
      cancelled = true
    }
  }, [params.id, router, user?.userId])

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
      setForm({ name: updated.name, document: updated.document, slug: updated.slug })
      setSaved(true)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar organização.'))
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
        title={organization.name}
        subtitle="Hub de identificação da organização — dados cadastrais e acesso ao tenant."
        breadcrumbs={[
          { label: 'Organizações', href: '/account/organizations' },
          { label: organization.name },
        ]}
        actions={
          <Button asChild>
            <a
              href={buildTenantUrl(form.slug, '/app')}
              target="_blank"
              rel="noreferrer"
            >
              Acessar organização
            </a>
          </Button>
        }
      />

      <div className="rounded-lg border bg-muted/20 p-4 text-sm">
        <p className="text-muted-foreground">Ambiente operacional</p>
        <a
          href={buildTenantUrl(form.slug, '/app')}
          className="font-medium text-primary hover:underline"
          target="_blank"
          rel="noreferrer"
        >
          {buildTenantUrl(form.slug, '/app')}
        </a>
      </div>

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        submitLabel="Salvar alterações"
        cancelHref="/account/organizations"
        error={error}
        success={saved ? 'Alterações salvas com sucesso.' : null}
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
          <div className="space-y-2">
            <Label htmlFor="slug">Slug</Label>
            <Input
              id="slug"
              value={form.slug}
              onChange={(event) =>
                setForm((prev) => ({ ...prev, slug: event.target.value }))
              }
              required
            />
          </div>
        </div>
      </FormContainer>
    </div>
  )
}
