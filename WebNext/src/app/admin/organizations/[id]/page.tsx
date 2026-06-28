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
import { buildTenantUrl } from '@/lib/tenant'

interface OrganizationDto {
  id: string
  name: string
  document: string
  slug: string
  rowVersion: string
}

export default function AdminOrganizationDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const [organization, setOrganization] = useState<OrganizationDto | null>(null)
  const [form, setForm] = useState({ name: '', document: '', slug: '' })
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)
  const [showDelete, setShowDelete] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)

  useEffect(() => {
    apiGet<OrganizationDto>(`api/organizations/${params.id}`)
      .then((org) => {
        setOrganization(org)
        setForm({ name: org.name, document: org.document, slug: org.slug })
      })
      .catch((err: ApiError) => {
        setError(formatApiErrorMessage(err, 'Organização não encontrada.'))
      })
      .finally(() => {
        setIsLoading(false)
      })
  }, [params.id])

  async function handleSubmit() {
    if (!organization) {
      return
    }

    setIsSubmitting(true)
    setError(null)
    setSuccess(null)

    try {
      const updated = await apiPut<OrganizationDto>(
        `api/organizations/${organization.id}`,
        {
          name: form.name,
          document: form.document,
          slug: form.slug,
          rowVersion: organization.rowVersion,
        },
      )
      setOrganization(updated)
      setForm({ name: updated.name, document: updated.document, slug: updated.slug })
      setSuccess('Alterações salvas com sucesso.')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar organização.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!organization) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organization.id}?rowVersion=${encodeURIComponent(organization.rowVersion)}`,
      )
      router.push('/admin/organizations')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir organização.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
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
        subtitle="Manutenção da organização pela plataforma."
        breadcrumbs={[
          { label: 'Organizações', href: '/admin/organizations' },
          { label: organization.name },
        ]}
        actions={
          <Button variant="destructive" size="sm" onClick={() => setShowDelete(true)}>
            Excluir
          </Button>
        }
      />

      <div className="rounded-lg border bg-muted/20 p-4 text-sm">
        <p className="text-muted-foreground">Acesso tenant</p>
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
        cancelHref="/admin/organizations"
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

      <ConfirmDialog
        open={showDelete}
        onOpenChange={setShowDelete}
        title="Excluir organização"
        description={`Confirma a exclusão de "${organization.name}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
