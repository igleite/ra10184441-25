'use client'

import Link from 'next/link'
import { useEffect, useState } from 'react'

import { EmptyState } from '@/components/shared/empty-state'
import { FormFeedback } from '@/components/shared/form-feedback'
import { Loading } from '@/components/shared/loading'
import { PageHeader } from '@/components/shared/page-header'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { apiGet } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { PermissionGate } from '@/components/shared/permission-gate'
import { usePermissions } from '@/hooks/use-permissions'
import { useTenant } from '@/providers/tenant-provider'

interface DashboardPlanDto {
  id: string
  description: string
  maxUsers: number
  maxClients: number
  maxTickets: number
}

export default function AppDashboardPage() {
  const { can } = usePermissions()
  const { organizationId, organization } = useTenant()
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [ticketCount, setTicketCount] = useState(0)
  const [customerCount, setCustomerCount] = useState(0)
  const [plan, setPlan] = useState<DashboardPlanDto | null>(null)
  const [error, setError] = useState<string | null>(null)

  const fetchKey = organizationId
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    async function loadDashboardData() {
      try {
        const [ticketsPage, customersPage, plansPage] = await Promise.all([
          apiGet<PageDto<unknown>>(
            `api/organizations/${organizationId}/tickets?pageIndex=1&pageSize=1`,
          ).catch((err: ApiError) => {
            if (err.statusCode === 404) {
              return {
                items: [],
                totalItemCount: 0,
                pageIndex: 1,
                pageSize: 1,
              } satisfies PageDto<unknown>
            }
            throw err
          }),
          apiGet<PageDto<unknown>>(
            `api/organizations/${organizationId}/customers?pageIndex=1&pageSize=1`,
          ).catch((err: ApiError) => {
            if (err.statusCode === 404) {
              return {
                items: [],
                totalItemCount: 0,
                pageIndex: 1,
                pageSize: 1,
              } satisfies PageDto<unknown>
            }
            throw err
          }),
          apiGet<PageDto<DashboardPlanDto>>(
            `api/organizations/${organizationId}/plans?pageIndex=1&pageSize=1`,
          ).catch((err: ApiError) => {
            if (err.statusCode === 404) {
              return {
                items: [],
                totalItemCount: 0,
                pageIndex: 1,
                pageSize: 1,
              } satisfies PageDto<DashboardPlanDto>
            }
            throw err
          }),
        ])

        if (!cancelled) {
          setTicketCount(ticketsPage.totalItemCount ?? 0)
          setCustomerCount(customersPage.totalItemCount ?? 0)
          setPlan(plansPage.items?.[0] ?? null)
          setError(null)
          setLoadedKey(fetchKey)
        }
      } catch (err) {
        if (!cancelled) {
          const apiError = err as ApiError
          setError(formatApiErrorMessage(apiError, 'Erro ao carregar dashboard.'))
          setLoadedKey(fetchKey)
        }
      }
    }

    void loadDashboardData()

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId])

  if (isLoading) {
    return <Loading fullPage label="Carregando dashboard..." />
  }

  return (
    <div className="space-y-8">
      <PageHeader
        title="Dashboard"
        subtitle={`Visão geral da organização ${organization?.name ?? ''}`.trim()}
      />

      {error ? <FormFeedback error={error} /> : null}

      <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Tickets</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-3xl font-semibold">{ticketCount}</p>
            <p className="text-xs text-muted-foreground">Total de tickets da organização</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="text-base">Clientes</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-3xl font-semibold">{customerCount}</p>
            <p className="text-xs text-muted-foreground">Total de clientes ativos na organização</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="text-base">Limites do plano</CardTitle>
          </CardHeader>
          <CardContent>
            {can('plans:read') ? (
              plan ? (
                <div className="space-y-1 text-sm">
                  <p>
                    <span className="font-medium">Usuários:</span> {plan.maxUsers}
                  </p>
                  <p>
                    <span className="font-medium">Clientes:</span> {plan.maxClients}
                  </p>
                  <p>
                    <span className="font-medium">Tickets:</span> {plan.maxTickets}
                  </p>
                </div>
              ) : (
                <p className="text-sm text-muted-foreground">Nenhum plano associado.</p>
              )
            ) : (
              <p className="text-sm text-muted-foreground">
                Limites do plano visíveis apenas para gestores da organização.
              </p>
            )}
          </CardContent>
        </Card>
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="text-base">Atalhos</CardTitle>
        </CardHeader>
        <CardContent className="flex flex-wrap gap-2">
          <Button variant="outline" size="sm" asChild>
            <Link href="/app/tickets">Tickets</Link>
          </Button>
          <Button variant="outline" size="sm" asChild>
            <Link href="/app/tickets/new">Novo ticket</Link>
          </Button>
          <Button variant="outline" size="sm" asChild>
            <Link href="/app/customers">Clientes</Link>
          </Button>
          <PermissionGate action="plans:read">
            <Button variant="outline" size="sm" asChild>
              <Link href="/app/plans">Planos</Link>
            </Button>
          </PermissionGate>
          <Button variant="outline" size="sm" asChild>
            <Link href="/app/profile">Perfil</Link>
          </Button>
        </CardContent>
      </Card>

      {!error && ticketCount === 0 && customerCount === 0 && !plan ? (
        <EmptyState
          title="Organização sem dados"
          description="Cadastre clientes, tickets e plano para começar a operar o app."
          action={
            can('plans:read')
              ? { label: 'Ir para planos', href: '/app/plans' }
              : { label: 'Ir para tickets', href: '/app/tickets' }
          }
        />
      ) : null}
    </div>
  )
}
