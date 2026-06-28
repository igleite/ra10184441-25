'use client'

import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useEffect, useMemo, useState } from 'react'

import { ConfirmDialog } from '@/components/shared/confirm-dialog'
import { DataTable, type ColumnDef } from '@/components/shared/data-table'
import { EmptyState } from '@/components/shared/empty-state'
import { EntityStatusBadge } from '@/components/shared/entity-status-badge'
import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { ListToolbar } from '@/components/shared/list-toolbar'
import { PageHeader } from '@/components/shared/page-header'
import { PlanCatalogPicker } from '@/components/shared/plan-catalog-picker'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiDelete, apiGet, apiPost } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { filterListByQuery } from '@/lib/filter-list'
import {
  findActiveOrganizationPlan,
  isOrganizationPlanActive,
  ORGANIZATION_PLAN_ASSOCIATE_BLOCKED_MESSAGE,
  ORGANIZATION_PLAN_ASSOCIATE_BLOCKED_TOOLTIP,
  type OrganizationPlanDto,
} from '@/lib/organization-plan'
import { PermissionGate } from '@/components/shared/permission-gate'
import { usePermissions } from '@/hooks/use-permissions'
import { useTenant } from '@/providers/tenant-provider'

interface OrganizationPlanAssociationState {
  hasActive: boolean
  activePlanDescription?: string
}

const PAGE_SIZE = 10
const ASSOCIATION_CHECK_PAGE_SIZE = 100

export default function AppPlansListPage() {
  const router = useRouter()
  const { can } = usePermissions()
  const { organizationId } = useTenant()
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [plans, setPlans] = useState<OrganizationPlanDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({
    planId: '',
    planLabel: '',
    description: '',
    maxUsers: '0',
    maxClients: '0',
    maxTickets: '0',
  })
  const [deleteTarget, setDeleteTarget] = useState<OrganizationPlanDto | null>(null)
  const [isDeleting, setIsDeleting] = useState(false)
  const [search, setSearch] = useState('')
  const [associationState, setAssociationState] =
    useState<OrganizationPlanAssociationState>({ hasActive: false })
  const [associationLoadedKey, setAssociationLoadedKey] = useState<string | null>(null)

  const canAssociatePlan = !associationState.hasActive
  const associationCheckKey = `${organizationId}-${reloadToken}`
  const isAssociationCheckLoading = associationLoadedKey !== associationCheckKey

  const filteredPlans = useMemo(
    () => filterListByQuery(plans, search, ['description', 'planId']),
    [plans, search],
  )

  const fetchKey = `${organizationId}-${pageIndex}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<OrganizationPlanDto>>(
      `api/organizations/${organizationId}/plans?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
    )
      .then((page) => {
        if (!cancelled) {
          setPlans(page.items ?? [])
          setTotalItemCount(page.totalItemCount ?? 0)
          setError(null)
          setLoadedKey(fetchKey)
        }
      })
      .catch((err: ApiError) => {
        if (!cancelled) {
          if (err.statusCode === 404) {
            setPlans([])
            setTotalItemCount(0)
            setError(null)
          } else {
            setError(formatApiErrorMessage(err, 'Erro ao carregar planos da organização.'))
          }
          setLoadedKey(fetchKey)
        }
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, pageIndex])

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<OrganizationPlanDto>>(
      `api/organizations/${organizationId}/plans?pageIndex=1&pageSize=${ASSOCIATION_CHECK_PAGE_SIZE}`,
    )
      .then((page) => {
        if (cancelled) {
          return
        }

        const activePlan = findActiveOrganizationPlan(page.items ?? [])
        setAssociationState({
          hasActive: Boolean(activePlan),
          activePlanDescription: activePlan?.description,
        })
        setAssociationLoadedKey(associationCheckKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        if (err.statusCode === 404) {
          setAssociationState({ hasActive: false })
          setAssociationLoadedKey(associationCheckKey)
          return
        }

        setError(formatApiErrorMessage(err, 'Erro ao verificar plano ativo da organização.'))
        setAssociationLoadedKey(associationCheckKey)
      })

    return () => {
      cancelled = true
    }
  }, [associationCheckKey, organizationId])

  async function handleCreate() {
    if (!canAssociatePlan) {
      setError(ORGANIZATION_PLAN_ASSOCIATE_BLOCKED_MESSAGE)
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const created = await apiPost<OrganizationPlanDto>(
        `api/organizations/${organizationId}/plans`,
        {
          planId: createForm.planId.trim(),
          description: can('plans:edit-description')
            ? createForm.description.trim()
            : (createForm.planLabel || createForm.description).trim(),
          maxUsers: can('plans:edit-limits') ? Number(createForm.maxUsers) : 0,
          maxClients: can('plans:edit-limits') ? Number(createForm.maxClients) : 0,
          maxTickets: can('plans:edit-limits') ? Number(createForm.maxTickets) : 0,
        },
      )
      setShowCreate(false)
      setCreateForm({
        planId: '',
        planLabel: '',
        description: '',
        maxUsers: '0',
        maxClients: '0',
        maxTickets: '0',
      })
      router.push(`/app/plans/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar vínculo de plano.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!deleteTarget) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/plans/${deleteTarget.id}?rowVersion=${encodeURIComponent(deleteTarget.rowVersion)}`,
      )
      setDeleteTarget(null)
      setShowCreate(false)
      setReloadToken((value) => value + 1)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir plano da organização.'))
    } finally {
      setIsDeleting(false)
    }
  }

  const columns: ColumnDef<OrganizationPlanDto>[] = [
    {
      id: 'status',
      header: 'Status',
      cell: (row) => (
        <EntityStatusBadge active={isOrganizationPlanActive(row)} />
      ),
    },
    {
      id: 'description',
      header: 'Descrição',
      cell: (row) => (
        <Link href={`/app/plans/${row.id}`} className="font-medium hover:underline">
          {row.description}
        </Link>
      ),
    },
    {
      id: 'limits',
      header: 'Limites',
      cell: (row) => `Usuários: ${row.maxUsers} • Clientes: ${row.maxClients} • Tickets: ${row.maxTickets}`,
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <div className="flex gap-2">
          <Button variant="outline" size="sm" asChild>
            <Link href={`/app/plans/${row.id}`}>Detalhes</Link>
          </Button>
          <PermissionGate action="plans:delete">
            <Button variant="destructive" size="sm" onClick={() => setDeleteTarget(row)}>
              Inativar
            </Button>
          </PermissionGate>
        </div>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Planos da organização"
        subtitle="Gerencie os vínculos de plano e os limites efetivos da operação."
        actions={
          <PermissionGate action="plans:link">
            {canAssociatePlan ? (
              <Button onClick={() => setShowCreate((value) => !value)}>
                {showCreate ? 'Cancelar' : 'Associar plano'}
              </Button>
            ) : (
              <span
                className="inline-flex cursor-not-allowed"
                title={ORGANIZATION_PLAN_ASSOCIATE_BLOCKED_TOOLTIP}
              >
                <Button disabled className="pointer-events-none">
                  Associar plano
                </Button>
              </span>
            )}
          </PermissionGate>
        }
      />

      {associationState.hasActive ? (
        <div
          className="rounded-md border border-amber-500/30 bg-amber-500/5 px-3 py-2 text-sm text-amber-900 dark:text-amber-200"
          role="alert"
        >
          {ORGANIZATION_PLAN_ASSOCIATE_BLOCKED_MESSAGE}
          {associationState.activePlanDescription ? (
            <span className="mt-1 block font-medium">
              Plano ativo: {associationState.activePlanDescription}
            </span>
          ) : null}
        </div>
      ) : null}

      <FormFeedback error={!showCreate ? error : null} />

      {showCreate && canAssociatePlan ? (
        <Card>
          <CardHeader>
            <CardTitle>Associar plano</CardTitle>
            <CardDescription>
              Vincule um plano do catálogo à organização. Só é permitido um plano ativo por vez.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <FormContainer
              onSubmit={handleCreate}
              isSubmitting={isSubmitting}
              submitLabel="Associar plano"
              error={error}
            >
              <div className="grid gap-4 sm:grid-cols-2">
                <div className="space-y-2 sm:col-span-2">
                  <PlanCatalogPicker
                    value={createForm.planId}
                    displayLabel={createForm.planLabel}
                    onChange={(planId, label) =>
                      setCreateForm((prev) => ({
                        ...prev,
                        planId,
                        planLabel: label,
                        description: prev.description || label,
                      }))
                    }
                    required
                  />
                </div>
                <div className="space-y-2 sm:col-span-2">
                  <Label htmlFor="description">Descrição</Label>
                  <Input
                    id="description"
                    value={createForm.description}
                    onChange={(event) =>
                      setCreateForm((prev) => ({ ...prev, description: event.target.value }))
                    }
                    disabled={!can('plans:edit-description')}
                    required
                  />
                  {!can('plans:edit-description') ? (
                    <p className="text-xs text-muted-foreground">
                      A descrição é definida pelo catálogo da plataforma. Apenas SuperAdmin pode
                      alterá-la.
                    </p>
                  ) : null}
                </div>
                {can('plans:edit-limits') ? (
                  <>
                    <div className="space-y-2">
                      <Label htmlFor="maxUsers">Máx. usuários</Label>
                      <Input
                        id="maxUsers"
                        type="number"
                        min={0}
                        value={createForm.maxUsers}
                        onChange={(event) =>
                          setCreateForm((prev) => ({ ...prev, maxUsers: event.target.value }))
                        }
                        required
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="maxClients">Máx. clientes</Label>
                      <Input
                        id="maxClients"
                        type="number"
                        min={0}
                        value={createForm.maxClients}
                        onChange={(event) =>
                          setCreateForm((prev) => ({ ...prev, maxClients: event.target.value }))
                        }
                        required
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="maxTickets">Máx. tickets</Label>
                      <Input
                        id="maxTickets"
                        type="number"
                        min={0}
                        value={createForm.maxTickets}
                        onChange={(event) =>
                          setCreateForm((prev) => ({ ...prev, maxTickets: event.target.value }))
                        }
                        required
                      />
                    </div>
                  </>
                ) : (
                  <p className="text-sm text-muted-foreground sm:col-span-2">
                    Descrição e limites do plano são definidos pela plataforma. Apenas SuperAdmin
                    pode configurá-los.
                  </p>
                )}
              </div>
            </FormContainer>
          </CardContent>
        </Card>
      ) : null}

      {plans.length === 0 && !isLoading && !isAssociationCheckLoading ? (
        <EmptyState
          title="Nenhum plano vinculado"
          description="Associe um plano do catálogo para definir os limites da organização."
          action={
            canAssociatePlan && can('plans:link')
              ? { label: 'Associar plano', onClick: () => setShowCreate(true) }
              : undefined
          }
        />
      ) : (
        <>
          <ListToolbar
            searchValue={search}
            onSearchChange={setSearch}
            searchPlaceholder="Buscar por descrição..."
          />
          <DataTable
            columns={columns}
            data={filteredPlans}
            isLoading={isLoading}
          emptyMessage="Nenhum plano vinculado."
          pagination={{
            pageIndex,
            pageSize: PAGE_SIZE,
            totalItemCount,
            onPageChange: setPageIndex,
          }}
        />
        </>
      )}

      <ConfirmDialog
        open={Boolean(deleteTarget)}
        onOpenChange={(open) => {
          if (!open) {
            setDeleteTarget(null)
          }
        }}
        title="Inativar vínculo de plano"
        description={`Confirma a inativação de "${deleteTarget?.description}"? Após inativar, será possível associar outro plano.`}
        confirmLabel={isDeleting ? 'Inativando...' : 'Inativar'}
        variant="destructive"
        onConfirm={handleDelete}
      />
    </div>
  )
}
