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

interface UserDto {
  id: string
  name: string
  email: string
  rowVersion: string
}

export default function AdminUserDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const [user, setUser] = useState<UserDto | null>(null)
  const [form, setForm] = useState({ name: '', email: '' })
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)
  const [showDelete, setShowDelete] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)

  useEffect(() => {
    apiGet<UserDto>(`api/users/${params.id}`)
      .then((loaded) => {
        setUser(loaded)
        setForm({ name: loaded.name, email: loaded.email })
      })
      .catch((err: ApiError) => {
        setError(formatApiErrorMessage(err, 'Usuário não encontrado.'))
      })
      .finally(() => {
        setIsLoading(false)
      })
  }, [params.id])

  async function handleSubmit() {
    if (!user) {
      return
    }

    setIsSubmitting(true)
    setError(null)
    setSuccess(null)

    try {
      const updated = await apiPut<UserDto>(`api/users/${user.id}`, {
        name: form.name,
        email: form.email,
        rowVersion: user.rowVersion,
      })
      setUser(updated)
      setSuccess('Alterações salvas com sucesso.')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar usuário.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!user) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/users/${user.id}?rowVersion=${encodeURIComponent(user.rowVersion)}`,
      )
      router.push('/admin/users')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir usuário.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <PageSkeleton fields={2} />
  }

  if (!user) {
    return <FormFeedback error={error ?? 'Usuário não encontrado.'} />
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={user.name}
        subtitle="Edição de conta global."
        breadcrumbs={[
          { label: 'Usuários', href: '/admin/users' },
          { label: user.name },
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
        cancelHref="/admin/users"
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
            <Label htmlFor="email">E-mail</Label>
            <Input
              id="email"
              type="email"
              value={form.email}
              onChange={(event) =>
                setForm((prev) => ({ ...prev, email: event.target.value }))
              }
              required
            />
          </div>
        </div>
      </FormContainer>

      <ConfirmDialog
        open={showDelete}
        onOpenChange={setShowDelete}
        title="Excluir usuário"
        description={`Confirma a exclusão de "${user.name}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
