'use client'

import Link from 'next/link'
import { useMemo, useState } from 'react'

import { FormContainer } from '@/components/shared/form-container'
import { PageHeader } from '@/components/shared/page-header'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiPost } from '@/lib/api/client'
import { rebootstrapSession } from '@/lib/auth/session'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import { buildTenantUrl } from '@/lib/tenant'

interface OrganizationDto {
  id: string
  name: string
  document: string
  slug: string
}

function normalizeSlug(value: string): string {
  return value
    .toLowerCase()
    .trim()
    .replace(/\s+/g, '-')
    .replace(/[^a-z0-9-]/g, '')
    .replace(/-+/g, '-')
    .replace(/^-|-$/g, '')
}

function isValidSlug(slug: string): boolean {
  return /^[a-z0-9]+(?:-[a-z0-9]+)*$/.test(slug)
}

export default function AccountOrganizationsNewPage() {
  const [form, setForm] = useState({ name: '', document: '', slug: '' })
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [slugTouched, setSlugTouched] = useState(false)

  const slugPreview = useMemo(() => normalizeSlug(form.slug), [form.slug])
  const slugError =
    slugTouched && slugPreview.length > 0 && !isValidSlug(slugPreview)
      ? 'Use apenas letras minúsculas, números e hífens.'
      : slugTouched && slugPreview.length === 0
        ? 'Informe um slug para o subdomínio.'
        : null

  async function handleSubmit() {
    const slug = normalizeSlug(form.slug)

    if (!isValidSlug(slug)) {
      setSlugTouched(true)
      setError('Revise o slug antes de continuar.')
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const created = await apiPost<OrganizationDto>('api/organizations', {
        name: form.name.trim(),
        document: form.document.trim(),
        slug,
      })
      await rebootstrapSession()
      window.location.assign('/account/organizations')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar organização.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title="Nova organização"
        subtitle="Cadastre uma organização vinculada à sua conta."
        breadcrumbs={[
          { label: 'Organizações', href: '/account/organizations' },
          { label: 'Nova' },
        ]}
      />

      {error ? (
        <p className="text-sm text-destructive" role="alert">
          {error}
        </p>
      ) : null}

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        submitLabel="Criar organização"
        cancelHref="/account/organizations"
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
            <Label htmlFor="slug">Slug (subdomínio)</Label>
            <Input
              id="slug"
              value={form.slug}
              onChange={(event) => {
                setSlugTouched(true)
                setForm((prev) => ({ ...prev, slug: event.target.value }))
              }}
              onBlur={() => setSlugTouched(true)}
              placeholder="minha-empresa"
              required
            />
            {slugError ? (
              <p className="text-sm text-destructive">{slugError}</p>
            ) : (
              <p className="text-xs text-muted-foreground">
                Prévia:{' '}
                {slugPreview ? (
                  <Link
                    href={buildTenantUrl(slugPreview, '/app')}
                    className="text-primary hover:underline"
                    target="_blank"
                    rel="noreferrer"
                  >
                    {buildTenantUrl(slugPreview, '/app')}
                  </Link>
                ) : (
                  'informe um slug válido'
                )}
              </p>
            )}
          </div>
        </div>
      </FormContainer>
    </div>
  )
}
