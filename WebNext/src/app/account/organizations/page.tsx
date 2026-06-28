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
import { filterMyOrganizations } from '@/lib/account-organizations'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { filterListByQuery } from '@/lib/filter-list'
import { buildTenantUrl } from '@/lib/tenant'
import { useAuth } from '@/providers/auth-provider'

interface OrganizationDto {
  id: string
  name: string
  document: string
  slug: string
}

const PAGE_SIZE = 10

export default function AccountOrganizationsListPage() {
  const { user } = useAuth()
  const [pageIndex, setPageIndex] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<OrganizationDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const [search, setSearch] = useState('')
  const fetchKey = `${pageIndex}-${user?.userId ?? 'anonymous'}`
  const isLoading = loadedKey !== fetchKey

  const filteredData = useMemo(
    () => filterListByQuery(data, search, ['name', 'slug']),
    [data, search],
  )

  useEffect(() => {
    if (!user?.userId) {
      return
    }

    let cancelled = false

    void apiGet<PageDto<OrganizationDto>>(
      `api/organizations?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
    )
      .then(async (page) => {
        if (cancelled) {
          return
        }

        const mine = await filterMyOrganizations(page.items ?? [], user.userId)
        setData(mine)
        setTotalItemCount(mine.length)
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

        setError(formatApiErrorMessage(err, 'Erro ao carregar organizações.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, pageIndex, user?.userId])

  const columns: ColumnDef<OrganizationDto>[] = [
    {
      id: 'name',
      header: 'Nome',
      cell: (row) => (
        <Link
          href={`/account/organizations/${row.id}`}
          className="font-medium hover:underline"
        >
          {row.name}
        </Link>
      ),
    },
    {
      id: 'document',
      header: 'Documento',
      cell: (row) => row.document,
    },
    {
      id: 'slug',
      header: 'Subdomínio',
      cell: (row) => (
        <a
          href={buildTenantUrl(row.slug, '/app')}
          className="text-primary hover:underline"
          target="_blank"
          rel="noreferrer"
        >
          {row.slug}
        </a>
      ),
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <Button variant="outline" size="sm" asChild>
          <Link href={`/account/organizations/${row.id}`}>Detalhes</Link>
        </Button>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Minhas organizações"
        subtitle="Organizações vinculadas à sua conta — não inclui orgs de outros donos."
        actions={
          <Button asChild>
            <Link href="/account/organizations/new">Nova organização</Link>
          </Button>
        }
      />

      <FormFeedback error={error} />

      {!isLoading && data.length === 0 ? (
        <EmptyState
          title="Nenhuma organização encontrada"
          description="Crie uma organização para começar a operar pelo subdomínio."
          action={{ label: 'Nova organização', href: '/account/organizations/new' }}
        />
      ) : (
        <>
          <ListToolbar
            searchValue={search}
            onSearchChange={setSearch}
            searchPlaceholder="Buscar por nome ou subdomínio..."
          />
          <DataTable
            columns={columns}
            data={filteredData}
            isLoading={isLoading}
          emptyMessage="Nenhuma organização vinculada à sua conta."
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
