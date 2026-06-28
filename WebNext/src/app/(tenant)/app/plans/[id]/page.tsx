'use client'

import { useParams, useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { EntityStatusBadge } from '@/components/shared/entity-status-badge'
import { EmptyState } from '@/components/shared/empty-state'
import { FormContainer } from '@/components/shared/form-container'
import { Loading } from '@/components/shared/loading'
import { PageHeader } from '@/components/shared/page-header'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiDelete, apiGet, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'
import { isOrganizationPlanActive, type OrganizationPlanDto } from '@/lib/organization-plan'
import { PermissionGate } from '@/components/shared/permission-gate'
import { usePermissions } from '@/hooks/use-permissions'
import { useTenant } from '@/providers/tenant-provider'

export default function AppPlanDetailPage() {
  const { can } = usePermissions()
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const [plan, setPlan] = useState<OrganizationPlanDto | null>(null)
  const [form, setForm] = useState({
    planId: '',
    description: '',
    maxUsers: '0',
    maxClients: '0',
    maxTickets: '0',
  })
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [showDelete, setShowDelete] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const fetchKey = `${organizationId}-${params.id}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<OrganizationPlanDto>(`api/organizations/${organizationId}/plans/${params.id}`)
      .then((loaded) => {
        if (!cancelled) {
          setPlan(loaded)
          setForm({
            planId: loaded.planId,
            description: loaded.description,
            maxUsers: String(loaded.maxUsers),
            maxClients: String(loaded.maxClients),
            maxTickets: String(loaded.maxTickets),
          })
          setError(null)
          setLoadedKey(fetchKey)
        }
      })
      .catch((err: ApiError) => {
        if (!cancelled) {
          setError(formatApiErrorMessage(err, 'Plano não encontrado.'))
          setLoadedKey(fetchKey)
        }
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, params.id])

  const canEditPlan = can('plans:edit-description') || can('plans:edit-limits')

  async function handleSubmit() {
    if (!plan || !canEditPlan) {
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const updated = await apiPut<OrganizationPlanDto>(
        `api/organizations/${organizationId}/plans/${plan.id}`,
        {
          planId: form.planId.trim(),
          description: can('plans:edit-description')
            ? form.description.trim()
            : plan.description,
          maxUsers: can('plans:edit-limits')
            ? Number(form.maxUsers)
            : plan.maxUsers,
          maxClients: can('plans:edit-limits')
            ? Number(form.maxClients)
            : plan.maxClients,
          maxTickets: can('plans:edit-limits')
            ? Number(form.maxTickets)
            : plan.maxTickets,
          rowVersion: plan.rowVersion,
        },
      )
      setPlan(updated)
      setForm({
        planId: updated.planId,
        description: updated.description,
        maxUsers: String(updated.maxUsers),
        maxClients: String(updated.maxClients),
        maxTickets: String(updated.maxTickets),
      })
      setReloadToken((value) => value + 1)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar plano.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!plan) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/plans/${plan.id}?rowVersion=${encodeURIComponent(plan.rowVersion)}`,
      )
      router.push('/app/plans')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir plano.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (isLoading) {
    return <Loading fullPage label="Carregando plano..." />
  }

  if (!plan) {
    return (
      <EmptyState
        title="Plano não encontrado"
        description={error ?? 'Não foi possível carregar o vínculo de plano.'}
        action={{ label: 'Voltar para planos', href: '/app/plans' }}
      />
    )
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={plan.description}
        subtitle={`Status: ${isOrganizationPlanActive(plan) ? 'Ativo' : 'Inativo'} — Limites: usuários ${plan.maxUsers}, clientes ${plan.maxClients}, tickets ${plan.maxTickets}`}
        breadcrumbs={[
          { label: 'Planos', href: '/app/plans' },
          { label: plan.description },
        ]}
        actions={
          <div className="flex items-center gap-2">
            <EntityStatusBadge active={isOrganizationPlanActive(plan)} />
            <PermissionGate action="plans:delete">
            <Button variant="destructive" size="sm" onClick={() => setShowDelete(true)}>
              Inativar
            </Button>
            </PermissionGate>
          </div>
        }
      />

      {error ? (
        <p className="text-sm text-destructive" role="alert">
          {error}
        </p>
      ) : null}

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        submitLabel="Salvar alterações"
        cancelHref="/app/plans"
        hideSubmit={!canEditPlan}
      >
        <div className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="planId">Plano do catálogo</Label>
            <Input id="planId" value={form.planId} readOnly disabled />
            <p className="text-xs text-muted-foreground">
              O vínculo com o catálogo global não pode ser alterado após a criação.
            </p>
          </div>
          <div className="space-y-2">
            <Label htmlFor="description">Descrição</Label>
            <Input
              id="description"
              value={form.description}
              onChange={(event) =>
                setForm((prev) => ({ ...prev, description: event.target.value }))
              }
              disabled={!can('plans:edit-description')}
              required
            />
            {!can('plans:edit-description') ? (
              <p className="text-xs text-muted-foreground">
                Apenas SuperAdmin pode alterar a descrição do plano.
              </p>
            ) : null}
          </div>
          <div className="grid gap-4 sm:grid-cols-3">
            <div className="space-y-2">
              <Label htmlFor="maxUsers">Máx. usuários</Label>
              <Input
                id="maxUsers"
                type="number"
                min={0}
                value={form.maxUsers}
                onChange={(event) =>
                  setForm((prev) => ({ ...prev, maxUsers: event.target.value }))
                }
                disabled={!can('plans:edit-limits')}
                required
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="maxClients">Máx. clientes</Label>
              <Input
                id="maxClients"
                type="number"
                min={0}
                value={form.maxClients}
                onChange={(event) =>
                  setForm((prev) => ({ ...prev, maxClients: event.target.value }))
                }
                disabled={!can('plans:edit-limits')}
                required
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="maxTickets">Máx. tickets</Label>
              <Input
                id="maxTickets"
                type="number"
                min={0}
                value={form.maxTickets}
                onChange={(event) =>
                  setForm((prev) => ({ ...prev, maxTickets: event.target.value }))
                }
                disabled={!can('plans:edit-limits')}
                required
              />
            </div>
          </div>
          {!can('plans:edit-limits') ? (
            <p className="text-xs text-muted-foreground">
              Limites e descrição do plano são definidos pela plataforma. Apenas SuperAdmin pode
              configurá-los.
            </p>
          ) : null}
        </div>
      </FormContainer>

      <ConfirmDialog
        open={showDelete}
        onOpenChange={setShowDelete}
        title="Inativar vínculo de plano"
        description={`Confirma a inativação de "${plan.description}"? Após inativar, será possível associar outro plano.`}
        confirmLabel={isDeleting ? 'Inativando...' : 'Inativar'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
