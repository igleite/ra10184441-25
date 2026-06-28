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
import { useAuth } from '@/providers/auth-provider'
import { useTenant } from '@/providers/tenant-provider'

interface UserDto {
  id: string
  name: string
  email: string
  rowVersion: string
}

export default function AppProfilePage() {
  const { user } = useAuth()
  const { organization } = useTenant()
  const userId = user?.userId ?? ''
  const [profile, setProfile] = useState<UserDto | null>(null)
  const [form, setForm] = useState({ name: '', email: '', password: '' })
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [saved, setSaved] = useState(false)

  const fetchKey = userId ? `${userId}-${reloadToken}` : null
  const isLoading = fetchKey ? loadedKey !== fetchKey : false

  useEffect(() => {
    if (!userId || !fetchKey) {
      return
    }

    let cancelled = false

    void apiGet<UserDto>(`api/users/${userId}`)
      .then((loaded) => {
        if (!cancelled) {
          setProfile(loaded)
          setForm({ name: loaded.name, email: loaded.email, password: '' })
          setError(null)
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
  }, [fetchKey, userId])

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
      setForm({ name: updated.name, email: updated.email, password: '' })
      setSaved(true)
      setReloadToken((value) => value + 1)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar perfil.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  if (isLoading) {
    return <PageSkeleton fields={3} />
  }

  if (!profile) {
    return <FormFeedback error={error ?? 'Usuário autenticado não encontrado.'} />
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title="Meu perfil"
        subtitle={`Perfil da área /app da organização ${organization?.name ?? ''}`.trim()}
      />

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        submitLabel="Salvar perfil"
        error={error}
        success={saved ? 'Perfil atualizado com sucesso.' : null}
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
            <Label htmlFor="email">E-mail</Label>
            <Input
              id="email"
              type="email"
              value={form.email}
              onChange={(event) => setForm((prev) => ({ ...prev, email: event.target.value }))}
              required
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="password">Senha</Label>
            <Input
              id="password"
              type="password"
              value={form.password}
              disabled
              placeholder="Alteração de senha indisponível"
            />
            <p className="text-xs text-muted-foreground">
              Não existe endpoint de alteração de senha na API para esta área.
            </p>
          </div>
        </div>
      </FormContainer>
    </div>
  )
}
