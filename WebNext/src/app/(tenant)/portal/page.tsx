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
import { useOrganizationTicketLabels } from '@/hooks/use-organization-ticket-labels'
import { useCustomer } from '@/providers/customer-provider'
import { useTenant } from '@/providers/tenant-provider'

interface TicketDto {
  id: string
  customerId: string
  description: string
  statusId: string
}

const FETCH_PAGE_SIZE = 100

export default function PortalDashboardPage() {
  const { organizationId, organization } = useTenant()
  const { customerId } = useCustomer()
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [ticketCount, setTicketCount] = useState(0)
  const [recentTickets, setRecentTickets] = useState<TicketDto[]>([])
  const [error, setError] = useState<string | null>(null)

  const labels = useOrganizationTicketLabels(organizationId)
  const fetchKey = `${organizationId}-${customerId ?? 'none'}`
  const isLoading = customerId ? loadedKey !== fetchKey : false

  useEffect(() => {
    if (!customerId) {
      return
    }

    let cancelled = false

    void apiGet<PageDto<TicketDto>>(
      `api/organizations/${organizationId}/tickets?pageIndex=1&pageSize=${FETCH_PAGE_SIZE}`,
    )
      .then((page) => {
        if (cancelled) {
          return
        }

        const customerTickets = (page.items ?? []).filter(
          (ticket) => ticket.customerId === customerId,
        )
        setTicketCount(customerTickets.length)
        setRecentTickets(customerTickets.slice(0, 5))
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        if (err.statusCode === 404) {
          setTicketCount(0)
          setRecentTickets([])
          setError(null)
          setLoadedKey(fetchKey)
          return
        }

        setError(formatApiErrorMessage(err, 'Erro ao carregar dashboard.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [customerId, fetchKey, organizationId])

  if (isLoading) {
    return <Loading fullPage label="Carregando dashboard..." />
  }

  return (
    <div className="space-y-8">
      <PageHeader
        title="Dashboard"
        subtitle={`Portal do cliente ${organization?.name ?? ''}`.trim()}
      />

      {error ? <FormFeedback error={error} /> : null}

      {!customerId ? (
        <EmptyState
          title="Cliente não identificado"
          description="O token de autenticação não possui customerId para filtrar os dados."
        />
      ) : (
        <>
          <div className="grid gap-4 sm:grid-cols-2">
            <Card>
              <CardHeader>
                <CardTitle className="text-base">Tickets do cliente</CardTitle>
              </CardHeader>
              <CardContent>
                <p className="text-3xl font-semibold">{ticketCount}</p>
                <p className="text-xs text-muted-foreground">
                  Chamados vinculados ao seu cliente
                </p>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle className="text-base">Atalhos</CardTitle>
              </CardHeader>
              <CardContent className="flex flex-wrap gap-2">
                <Button variant="outline" size="sm" asChild>
                  <Link href="/portal/tickets">Ver tickets</Link>
                </Button>
                <Button variant="outline" size="sm" asChild>
                  <Link href="/portal/tickets/new">Novo ticket</Link>
                </Button>
                <Button variant="outline" size="sm" asChild>
                  <Link href="/portal/artifacts">Artefatos</Link>
                </Button>
                <Button variant="outline" size="sm" asChild>
                  <Link href="/portal/profile">Perfil</Link>
                </Button>
              </CardContent>
            </Card>
          </div>

          <Card>
            <CardHeader>
              <CardTitle className="text-base">Tickets recentes</CardTitle>
            </CardHeader>
            <CardContent>
              {recentTickets.length === 0 ? (
                <EmptyState
                  title="Nenhum ticket recente"
                  description="Abra o primeiro chamado para acompanhar o atendimento."
                  action={{ label: 'Novo ticket', href: '/portal/tickets/new' }}
                />
              ) : (
                <ul className="space-y-2">
                  {recentTickets.map((ticket) => (
                    <li key={ticket.id}>
                      <Link
                        href={`/portal/tickets/${ticket.id}`}
                        className="text-sm font-medium hover:underline"
                      >
                        {ticket.description}
                      </Link>
                      <p className="text-xs text-muted-foreground">
                        Status: {labels.statusName(ticket.statusId)}
                      </p>
                    </li>
                  ))}
                </ul>
              )}
            </CardContent>
          </Card>
        </>
      )}
    </div>
  )
}
