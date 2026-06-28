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
import { useTenant } from '@/providers/tenant-provider'

interface TicketDto {
  id: string
  customerId: string
  statusId: string
  description: string
}

const PAGE_SIZE = 10

export default function AppTicketsListPage() {
  const { organizationId } = useTenant()
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<TicketDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${pageIndex}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey
  const [search, setSearch] = useState('')
  const [sortField, setSortField] = useState<'description' | 'customerId' | 'statusId'>('description')
  const [sortDirection, setSortDirection] = useState<'asc' | 'desc'>('asc')
  const labels = useOrganizationTicketLabels(organizationId)

  const filteredData = useMemo(
    () =>
      queryList(data, {
        search,
        searchFields: ['description', 'customerId', 'statusId'],
        sortField,
        sortDirection,
      }),
    [data, search, sortDirection, sortField],
  )

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<TicketDto>>(
      `api/organizations/${organizationId}/tickets?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
    )
      .then((page) => {
        if (cancelled) {
          return
        }

        setData(page.items ?? [])
        setTotalItemCount(page.totalItemCount)
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        if (err.statusCode === 404) {
          setData([])
          setTotalItemCount(0)
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
  }, [fetchKey, organizationId, pageIndex, reloadToken])

  const columns: ColumnDef<TicketDto>[] = [
    {
      id: 'description',
      header: 'Descrição',
      cell: (row) => (
        <Link href={`/app/tickets/${row.id}`} className="font-medium hover:underline">
          {row.description}
        </Link>
      ),
    },
    {
      id: 'customerId',
      header: 'Cliente',
      cell: (row) => labels.customerName(row.customerId),
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
          <Link href={`/app/tickets/${row.id}`}>Detalhes</Link>
        </Button>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Tickets"
        subtitle="Acompanhamento dos chamados da organização."
        actions={
          <div className="flex gap-2">
            <Button variant="outline" onClick={() => setReloadToken((value) => value + 1)}>
              Atualizar
            </Button>
            <Button asChild>
              <Link href="/app/tickets/new">Novo ticket</Link>
            </Button>
          </div>
        }
      />

      <FormFeedback error={error} />

      {!isLoading && data.length === 0 ? (
        <EmptyState
          title="Nenhum ticket encontrado"
          description="Crie um ticket para iniciar o atendimento."
          action={{ label: 'Novo ticket', href: '/app/tickets/new' }}
        />
      ) : (
        <>
          <ListToolbar
            searchValue={search}
            onSearchChange={setSearch}
            searchPlaceholder="Buscar por descrição, cliente ou status..."
            sortField={sortField}
            onSortFieldChange={(value) =>
              setSortField(value as 'description' | 'customerId' | 'statusId')
            }
            sortDirection={sortDirection}
            onSortDirectionChange={setSortDirection}
            sortOptions={[
              { value: 'description', label: 'Descrição' },
              { value: 'customerId', label: 'Cliente' },
              { value: 'statusId', label: 'Status' },
            ]}
          />
          <DataTable
            columns={columns}
            data={filteredData}
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
