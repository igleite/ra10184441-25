'use client'

import { useParams, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { PageHeader } from '@/components/shared/page-header'
import { PageSkeleton } from '@/components/shared/page-skeleton'
import { SelectField } from '@/components/shared/select-field'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiDelete, apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import { TEAM_ROLE_OPTIONS } from '@/lib/role-options'
import { useTenant } from '@/providers/tenant-provider'

interface TeamDto {
  id: string
  name: string
  code: string
  roleId: string
  rowVersion: string
}

export default function AppTeamDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const [team, setTeam] = useState<TeamDto | null>(null)
  const [form, setForm] = useState({ name: '', code: '', roleId: '' })
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)
  const [showDelete, setShowDelete] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)

  useEffect(() => {
    apiGet<TeamDto>(`api/organizations/${organizationId}/teams/${params.id}`)
      .then((loaded) => {
        setTeam(loaded)
        setForm({
          name: loaded.name,
          code: loaded.code,
          roleId: loaded.roleId,
        })
      })
      .catch((err: ApiError) => {
        setError(formatApiErrorMessage(err, 'Time não encontrado.'))
      })
      .finally(() => {
        setIsLoading(false)
      })
  }, [organizationId, params.id])

  async function handleSubmit() {
    if (!team) {
      return
    }

    setIsSubmitting(true)
    setError(null)
    setSuccess(null)

    try {
      const updated = await apiPut<TeamDto>(
        `api/organizations/${organizationId}/teams/${team.id}`,
        {
          name: form.name.trim(),
          code: form.code.trim(),
          roleId: form.roleId.trim(),
          rowVersion: team.rowVersion,
        },
      )
      setTeam(updated)
      setForm({
        name: updated.name,
        code: updated.code,
        roleId: updated.roleId,
      })
      setSuccess('Alterações salvas com sucesso.')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar time.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!team) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/teams/${team.id}?rowVersion=${encodeURIComponent(team.rowVersion)}`,
      )
      router.push('/app/teams')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir time.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <PageSkeleton fields={3} />
  }

  if (!team) {
    return <FormFeedback error={error ?? 'Time não encontrado.'} />
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={team.name}
        subtitle="Edição de time da organização."
        breadcrumbs={[
          { label: 'Times', href: '/app/teams' },
          { label: team.name },
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
        cancelHref="/app/teams"
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
            <Label htmlFor="code">Código</Label>
            <Input
              id="code"
              value={form.code}
              onChange={(event) =>
                setForm((prev) => ({ ...prev, code: event.target.value }))
              }
              required
            />
          </div>
          <SelectField
            id="roleId"
            label="Papel"
            value={form.roleId}
            onChange={(value) => setForm((prev) => ({ ...prev, roleId: value }))}
            options={[...TEAM_ROLE_OPTIONS]}
            placeholder="Selecione o papel..."
            required
          />
        </div>
      </FormContainer>

      <ConfirmDialog
        open={showDelete}
        onOpenChange={setShowDelete}
        title="Excluir time"
        description={`Confirma a exclusão de "${team.name}"?`}
        confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
