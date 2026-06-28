'use client'

import Link from 'next/link'
import { useEffect, useMemo, useState } from 'react'

import { DataTable, type ColumnDef } from '@/components/shared/data-table'
import { EmptyState } from '@/components/shared/empty-state'
import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { ListToolbar } from '@/components/shared/list-toolbar'
import { PageHeader } from '@/components/shared/page-header'
import { SelectField } from '@/components/shared/select-field'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { apiGet, apiPost } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { filterListByQuery } from '@/lib/filter-list'
import { PermissionGate } from '@/components/shared/permission-gate'
import { usePermissions } from '@/hooks/use-permissions'
import { useCustomer } from '@/providers/customer-provider'
import { useTenant } from '@/providers/tenant-provider'

interface CustomerArtifactDto {
  id: string
  customerId: string
  artifactId: string
  rowVersion: string
}

interface CatalogArtifactDto {
  id: string
  name: string
  code: string
}

interface ArtifactTypeDto {
  id: string
  name: string
}

const PAGE_SIZE = 10
const CATALOG_PAGE_SIZE = 100

export default function PortalArtifactsListPage() {
  const { organizationId } = useTenant()
  const { customerId } = useCustomer()
  const { can } = usePermissions()
  const canManageArtifacts = can('portal-artifacts:manage')
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<CustomerArtifactDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({ artifactId: '' })
  const [catalogArtifacts, setCatalogArtifacts] = useState<CatalogArtifactDto[]>([])
  const [search, setSearch] = useState('')

  const fetchKey = `${organizationId}-${customerId ?? 'none'}-${pageIndex}-${reloadToken}`
  const isLoading = customerId ? loadedKey !== fetchKey : false

  const artifactLabelById = useMemo(
    () =>
      new Map(
        catalogArtifacts.map((artifact) => [
          artifact.id,
          `${artifact.name} (${artifact.code})`,
        ]),
      ),
    [catalogArtifacts],
  )

  const catalogOptions = useMemo(
    () =>
      catalogArtifacts.map((artifact) => ({
        value: artifact.id,
        label: `${artifact.name} (${artifact.code})`,
      })),
    [catalogArtifacts],
  )

  const filteredData = useMemo(() => {
    const byQuery = filterListByQuery(data, search, ['artifactId', 'id'])
    const normalized = search.trim().toLowerCase()
    if (!normalized) {
      return byQuery
    }
    return byQuery.filter((row) => {
      const label = artifactLabelById.get(row.artifactId) ?? ''
      return label.toLowerCase().includes(normalized)
    })
  }, [data, search, artifactLabelById])

  useEffect(() => {
    if (!customerId) {
      return
    }

    let cancelled = false

    void apiGet<PageDto<CustomerArtifactDto>>(
      `api/organizations/${organizationId}/customers/${customerId}/artifacts?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
    )
      .then((page) => {
        if (cancelled) {
          return
        }

        const customerArtifacts = (page.items ?? []).filter(
          (item) => item.customerId === customerId,
        )
        setData(customerArtifacts)
        setTotalItemCount(page.totalItemCount ?? customerArtifacts.length)
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        setError(formatApiErrorMessage(err, 'Erro ao carregar artefatos do cliente.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [customerId, fetchKey, organizationId, pageIndex])

  useEffect(() => {
    let cancelled = false

    async function loadCatalog() {
      try {
        const typesPage = await apiGet<PageDto<ArtifactTypeDto>>(
          `api/organizations/${organizationId}/artifact-types?pageIndex=1&pageSize=${CATALOG_PAGE_SIZE}`,
        )
        const allArtifacts: CatalogArtifactDto[] = []

        for (const type of typesPage.items ?? []) {
          const artifactsPage = await apiGet<PageDto<CatalogArtifactDto>>(
            `api/organizations/${organizationId}/artifact-types/${type.id}/artifacts?pageIndex=1&pageSize=${CATALOG_PAGE_SIZE}`,
          ).catch(() => ({
            items: [],
            totalItemCount: 0,
            pageIndex: 1,
            pageSize: CATALOG_PAGE_SIZE,
          } satisfies PageDto<CatalogArtifactDto>))
          allArtifacts.push(...(artifactsPage.items ?? []))
        }

        if (!cancelled) {
          setCatalogArtifacts(allArtifacts)
        }
      } catch {
        if (!cancelled) {
          setCatalogArtifacts([])
        }
      }
    }

    void loadCatalog()

    return () => {
      cancelled = true
    }
  }, [organizationId])

  async function handleCreate() {
    if (!customerId || !canManageArtifacts) {
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      await apiPost<CustomerArtifactDto>(
        `api/organizations/${organizationId}/customers/${customerId}/artifacts`,
        {
          artifactId: createForm.artifactId.trim(),
        },
      )
      setShowCreate(false)
      setCreateForm({ artifactId: '' })
      setReloadToken((value) => value + 1)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar vínculo de artefato.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  const columns: ColumnDef<CustomerArtifactDto>[] = [
    {
      id: 'artifactId',
      header: 'Artefato',
      cell: (row) => (
        <Link href={`/portal/artifacts/${row.id}`} className="font-medium hover:underline">
          {artifactLabelById.get(row.artifactId) ?? row.artifactId}
        </Link>
      ),
    },
    {
      id: 'actions',
      header: 'Ações',
      cell: (row) => (
        <Button variant="outline" size="sm" asChild>
          <Link href={`/portal/artifacts/${row.id}`}>Detalhes</Link>
        </Button>
      ),
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Artefatos"
        subtitle={
          canManageArtifacts
            ? 'Leitura e edição dos artefatos do seu cliente.'
            : 'Visualização dos artefatos do seu cliente.'
        }
        actions={
          <div className="flex gap-2">
            <Button variant="outline" onClick={() => setReloadToken((value) => value + 1)}>
              Atualizar
            </Button>
            <PermissionGate action="portal-artifacts:manage">
              <Button onClick={() => setShowCreate((value) => !value)}>
                {showCreate ? 'Cancelar' : 'Novo vínculo'}
              </Button>
            </PermissionGate>
          </div>
        }
      />

      <FormFeedback error={!showCreate ? error : null} />

      {!customerId ? (
        <EmptyState
          title="Cliente não identificado"
          description="Não foi possível carregar artefatos sem customerId no token."
        />
      ) : null}

      {customerId && canManageArtifacts && showCreate ? (
        <Card>
          <CardHeader>
            <CardTitle>Novo vínculo de artefato</CardTitle>
            <CardDescription>Selecione o artefato do catálogo da organização.</CardDescription>
          </CardHeader>
          <CardContent>
            <FormContainer
              onSubmit={handleCreate}
              isSubmitting={isSubmitting}
              submitLabel="Criar"
              error={error}
            >
              <SelectField
                id="artifactId"
                label="Artefato"
                value={createForm.artifactId}
                onChange={(value) => setCreateForm({ artifactId: value })}
                options={catalogOptions}
                placeholder={
                  catalogOptions.length === 0
                    ? 'Nenhum artefato no catálogo'
                    : 'Selecione o artefato...'
                }
                required
                disabled={catalogOptions.length === 0}
              />
            </FormContainer>
          </CardContent>
        </Card>
      ) : null}

      {customerId ? (
        <>
          <ListToolbar
            searchValue={search}
            onSearchChange={setSearch}
            searchPlaceholder="Buscar por artefato..."
          />
          <DataTable
            columns={columns}
            data={filteredData}
            isLoading={isLoading}
            emptyMessage="Nenhum artefato vinculado."
            pagination={{
              pageIndex,
              pageSize: PAGE_SIZE,
              totalItemCount,
              onPageChange: setPageIndex,
            }}
          />
        </>
      ) : null}
    </div>
  )
}
