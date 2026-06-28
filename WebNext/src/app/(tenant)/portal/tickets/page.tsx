'use client'

import Link from 'next/link'
import { useEffect, useMemo, useState } from 'react'

import { DataTable, type ColumnDef } from '@/components/shared/data-table'
import { EmptyState } from '@/components/shared/empty-state'
import { FormFeedback } from '@/components/shared/form-feedback'
import { ListToolbar } from '@/components/shared/list-toolbar'
import { PageHeader } from '@/components/shared/page-header'
import { Button } from '@/components/ui/button'
import { apiGet } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { queryList } from '@/lib/list-query'
import { useOrganizationTicketLabels } from '@/hooks/use-organization-ticket-labels'
import { useCustomer } from '@/providers/customer-provider'
import { useTenant } from '@/providers/tenant-provider'

interface TicketDto {
  id: string
  customerId: string
  statusId: string
  description: string
}

const FETCH_PAGE_SIZE = 100
const PAGE_SIZE = 10

export default function PortalTicketsListPage() {
  const { organizationId } = useTenant()
  const { customerId } = useCustomer()
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [allTickets, setAllTickets] = useState<TicketDto[]>([])
  const [error, setError] = useState<string | null>(null)
  const [search, setSearch] = useState('')
  const labels = useOrganizationTicketLabels(organizationId)

  const filteredTickets = useMemo(() => {
    const base = customerId
      ? allTickets.filter((ticket) => ticket.customerId === customerId)
      : []
    return queryList(base, {
      search,
      searchFields: ['description', 'statusId'],
      sortField: 'description',
      sortDirection: 'asc',
    })
  }, [allTickets, customerId, search])

  const fetchKey = `${organizationId}-${customerId ?? 'none'}-${reloadToken}`
  const isLoading = customerId ? loadedKey !== fetchKey : false

  const pageStart = pageIndex * PAGE_SIZE
  const data = filteredTickets.slice(pageStart, pageStart + PAGE_SIZE)
  const totalItemCount = filteredTickets.length

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

        setAllTickets(page.items ?? [])
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        if (err.statusCode === 404) {
          setAllTickets([])
          setError(null)
          setLoadedKey(fetchKey)
          return
        }

        setError(formatApiErrorMessage(err, 'Erro ao carregar tickets.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [customerId, fetchKey, organizationId])

  const columns: ColumnDef<TicketDto>[] = [
    {
      id: 'description',
      header: 'Descrição',
      cell: (row) => (
        <Link href={`/portal/tickets/${row.id}`} className="font-medium hover:underline">
          {row.description}
        </Link>
      ),
    },
    {
      id: 'statusId',
      header: 'Status',
      cell: (row) => labels.statusName(row.statusId),
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <Button variant="outline" size="sm" asChild>
          <Link href={`/portal/tickets/${row.id}`}>Detalhes</Link>
        </Button>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Tickets"
        subtitle="Chamados do seu cliente."
        actions={
          <div className="flex gap-2">
            <Button variant="outline" onClick={() => setReloadToken((value) => value + 1)}>
              Atualizar
            </Button>
            <Button asChild>
              <Link href="/portal/tickets/new">Novo ticket</Link>
            </Button>
          </div>
        }
      />

      <FormFeedback error={error} />

      {!customerId ? (
        <EmptyState
          title="Cliente não identificado"
          description="Não foi possível filtrar tickets sem customerId no token."
        />
      ) : !isLoading && filteredTickets.length === 0 && !search.trim() ? (
        <EmptyState
          title="Nenhum ticket encontrado"
          description="Abra um chamado para iniciar o atendimento."
          action={{ label: 'Novo ticket', href: '/portal/tickets/new' }}
        />
      ) : (
        <>
          <ListToolbar
            searchValue={search}
            onSearchChange={setSearch}
            searchPlaceholder="Buscar por descrição ou status..."
          />
          <DataTable
            columns={columns}
            data={data}
            isLoading={isLoading}
          emptyMessage="Nenhum ticket cadastrado."
          pagination={{
            pageIndex,
            pageSize: PAGE_SIZE,
            totalItemCount,
            onPageChange: setPageIndex,
          }}
        />
        </>
      )}
    </div>
  )
}
