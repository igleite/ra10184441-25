'use client'

import { useParams, useRouter } from 'next/navigation'
import { useEffect, useMemo, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { EmptyState } from '@/components/shared/empty-state'
import { FormContainer } from '@/components/shared/form-container'
import { PageHeader } from '@/components/shared/page-header'
import { PageSkeleton } from '@/components/shared/page-skeleton'
import { PlatformUserPicker } from '@/components/shared/platform-user-picker'
import { SelectField } from '@/components/shared/select-field'
import { Button } from '@/components/ui/button'
import { apiDelete, apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { useTenant } from '@/providers/tenant-provider'

interface OrganizationUserDto {
  id: string
  userId: string
  teamId: string
  rowVersion: string
}

interface TeamDto {
  id: string
  name: string
}

const TEAM_PAGE_SIZE = 50

export default function AppUserDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const [organizationUser, setOrganizationUser] = useState<OrganizationUserDto | null>(null)
  const [teams, setTeams] = useState<TeamDto[]>([])
  const [form, setForm] = useState({ userId: '', teamId: '', userLabel: '' })
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [showDelete, setShowDelete] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)

  const fetchKey = `${organizationId}-${params.id}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey

  const teamOptions = useMemo(
    () => teams.map((team) => ({ value: team.id, label: team.name })),
    [teams],
  )

  useEffect(() => {
    let cancelled = false

    async function loadData() {
      try {
        const [loaded, teamsPage] = await Promise.all([
          apiGet<OrganizationUserDto>(`api/organizations/${organizationId}/users/${params.id}`),
          apiGet<PageDto<TeamDto>>(
            `api/organizations/${organizationId}/teams?pageIndex=1&pageSize=${TEAM_PAGE_SIZE}`,
          ).catch(() => ({
            items: [],
            totalItemCount: 0,
            pageIndex: 1,
            pageSize: TEAM_PAGE_SIZE,
          } satisfies PageDto<TeamDto>)),
        ])

        if (!cancelled) {
          setOrganizationUser(loaded)
          setForm({ userId: loaded.userId, teamId: loaded.teamId, userLabel: loaded.userId })
          setTeams(teamsPage.items ?? [])
          setError(null)
          setLoadedKey(fetchKey)
        }
      } catch (err) {
        if (!cancelled) {
          setError(formatApiErrorMessage(err as ApiError, 'Usuário da organização não encontrado.'))
          setLoadedKey(fetchKey)
        }
      }
    }

    void loadData()

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, params.id])

  async function handleSubmit() {
    if (!organizationUser) {
      return
    }

    setIsSubmitting(true)
    setError(null)
    setSuccess(null)

    try {
      const updated = await apiPut<OrganizationUserDto>(
        `api/organizations/${organizationId}/users/${organizationUser.id}`,
        {
          userId: form.userId.trim(),
          teamId: form.teamId.trim(),
          rowVersion: organizationUser.rowVersion,
        },
      )
      setOrganizationUser(updated)
      setForm({
        userId: updated.userId,
        teamId: updated.teamId,
        userLabel: form.userLabel || updated.userId,
      })
      setSuccess('Alterações salvas com sucesso.')
      setReloadToken((value) => value + 1)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar usuário da organização.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!organizationUser) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/users/${organizationUser.id}?rowVersion=${encodeURIComponent(organizationUser.rowVersion)}`,
      )
      router.push('/app/users')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao remover usuário da organização.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <PageSkeleton fields={2} />
  }

  if (!organizationUser) {
    return (
      <EmptyState
        title="Usuário não encontrado"
        description={error ?? 'Não foi possível carregar este vínculo de usuário.'}
        action={{ label: 'Voltar para usuários', href: '/app/users' }}
      />
    )
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title="Usuário da organização"
        subtitle={`Vínculo ${organizationUser.userId}`}
        breadcrumbs={[
          { label: 'Usuários', href: '/app/users' },
          { label: organizationUser.userId },
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
        submitLabel="Salvar alterações"
        cancelHref="/app/users"
        error={error}
        success={success}
      >
        <div className="space-y-4">
          <PlatformUserPicker
            value={form.userId}
            displayLabel={form.userLabel}
            onChange={(userId, label) =>
              setForm((prev) => ({ ...prev, userId, userLabel: label }))
            }
            required
          />
          <SelectField
            id="teamId"
            label="Time"
            value={form.teamId}
            onChange={(value) => setForm((prev) => ({ ...prev, teamId: value }))}
            options={teamOptions}
            placeholder="Selecione o time..."
            required
            disabled={teamOptions.length === 0}
          />
        </div>
      </FormContainer>

      <ConfirmDialog
        open={showDelete}
        onOpenChange={setShowDelete}
        title="Remover usuário da organização"
        description={`Confirma a remoção do usuário ${organizationUser.userId}?`}
        confirmLabel={isDeleting ? 'Removendo...' : 'Remover'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
