'use client'

import { useEffect, useState } from 'react'

import { FormContainer } from '@/components/shared/form-container'
import { PageSkeleton } from '@/components/shared/page-skeleton'
import { PageHeader } from '@/components/shared/page-header'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import { useAuth } from '@/providers/auth-provider'

interface UserDto {
  id: string
  name: string
  email: string
  rowVersion: string
}

export default function AccountProfilePage() {
  const { user } = useAuth()
  const [profile, setProfile] = useState<UserDto | null>(null)
  const [form, setForm] = useState({ name: '', email: '' })
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [saved, setSaved] = useState(false)
  const fetchKey = user?.userId ?? 'anonymous'
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    if (!user?.userId) {
      return
    }

    let cancelled = false

    void apiGet<UserDto>(`api/users/${user.userId}`)
      .then((loaded) => {
        if (!cancelled) {
          setProfile(loaded)
          setForm({ name: loaded.name, email: loaded.email })
          setLoadedKey(fetchKey)
        }
      })
      .catch((err: ApiError) => {
        if (!cancelled) {
          setError(formatApiErrorMessage(err, 'Erro ao carregar perfil.'))
          setLoadedKey(fetchKey)
        }
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, user?.userId])

  async function handleSubmit() {
    if (!profile) {
      return
    }

    setIsSubmitting(true)
    setError(null)
    setSaved(false)

    try {
      const updated = await apiPut<UserDto>(`api/users/${profile.id}`, {
        name: form.name.trim(),
        email: form.email.trim(),
        rowVersion: profile.rowVersion,
      })
      setProfile(updated)
      setForm((prev) => ({ ...prev, name: updated.name, email: updated.email }))
      setSaved(true)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar perfil.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  if (isLoading) {
    return <PageSkeleton fields={2} />
  }

  if (!profile) {
    return (
      <p className="text-sm text-destructive" role="alert">
        {error ?? 'Perfil não encontrado.'}
      </p>
    )
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title="Perfil global"
        subtitle="Conta da plataforma (domínio raiz) — distinto de /app/profile e /portal/profile."
      />

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        submitLabel="Salvar perfil"
        cancelHref="/account"
        error={error}
        success={saved ? 'Perfil atualizado com sucesso.' : null}
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
    </div>
  )
}
