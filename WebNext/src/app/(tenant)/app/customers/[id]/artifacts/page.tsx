'use client'

import { useParams } from 'next/navigation'
import { useEffect, useMemo, useState } from 'react'

import { ArtifactCatalogPicker } from '@/components/shared/artifact-catalog-picker'
import { DataTable, type ColumnDef } from '@/components/shared/data-table'
import { FormContainer } from '@/components/shared/form-container'
import { FormFeedback } from '@/components/shared/form-feedback'
import { ListToolbar } from '@/components/shared/list-toolbar'
import { PageHeader } from '@/components/shared/page-header'
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
import { artifactCatalogLabel, fetchOrganizationArtifactCatalog } from '@/lib/artifact-catalog'
import { useTenant } from '@/providers/tenant-provider'

interface CustomerArtifactDto {
  id: string
  customerId: string
  artifactId: string
  rowVersion: string
}

const PAGE_SIZE = 10

export default function AppCustomerArtifactsListPage() {
  const params = useParams<{ id: string }>()
  const { organizationId } = useTenant()
  const [pageIndex, setPageIndex] = useState(0)
  const [reloadToken, setReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [data, setData] = useState<CustomerArtifactDto[]>([])
  const [totalItemCount, setTotalItemCount] = useState(0)
  const [error, setError] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${params.id}-${pageIndex}-${reloadToken}`
  const isLoading = loadedKey !== fetchKey
  const [showCreate, setShowCreate] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [createForm, setCreateForm] = useState({ artifactId: '' })
  const [search, setSearch] = useState('')
  const [artifactLabels, setArtifactLabels] = useState<Map<string, string>>(new Map())

  const filteredData = useMemo(
    () => {
      const byQuery = filterListByQuery(data, search, ['artifactId', 'id'])
      const normalized = search.trim().toLowerCase()
      if (!normalized) {
        return byQuery
      }
      return byQuery.filter((row) => {
        const label = artifactLabels.get(row.artifactId) ?? ''
        return label.toLowerCase().includes(normalized)
      })
    },
    [artifactLabels, data, search],
  )

  useEffect(() => {
    let cancelled = false

    void fetchOrganizationArtifactCatalog(organizationId)
      .then((catalog) => {
        if (!cancelled) {
          setArtifactLabels(
            new Map(catalog.map((artifact) => [artifact.id, artifactCatalogLabel(artifact)])),
          )
        }
      })
      .catch(() => {
        if (!cancelled) {
          setArtifactLabels(new Map())
        }
      })

    return () => {
      cancelled = true
    }
  }, [organizationId])

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<CustomerArtifactDto>>(
      `api/organizations/${organizationId}/customers/${params.id}/artifacts?pageIndex=${pageIndex + 1}&pageSize=${PAGE_SIZE}`,
    )
      .then((page) => {
        if (!cancelled) {
          setData(page.items ?? [])
          setTotalItemCount(page.totalItemCount)
          setError(null)
          setLoadedKey(fetchKey)
        }
      })
      .catch((err: ApiError) => {
        if (!cancelled) {
          setError(formatApiErrorMessage(err, 'Erro ao carregar artefatos do cliente.'))
          setLoadedKey(fetchKey)
        }
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, organizationId, pageIndex, params.id])

  async function handleCreate() {
    setIsSubmitting(true)
    setError(null)

    try {
      await apiPost<CustomerArtifactDto>(
        `api/organizations/${organizationId}/customers/${params.id}/artifacts`,
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
      id: 'id',
      header: 'ID',
      cell: (row) => row.id,
    },
    {
      id: 'artifactId',
      header: 'Artefato',
      cell: (row) => artifactLabels.get(row.artifactId) ?? row.artifactId,
    },
  ]

  return (
    <div className="space-y-8">
      <PageHeader
        title="Artefatos do cliente"
        subtitle="Vínculos de artefatos no contexto do cliente."
        breadcrumbs={[
          { label: 'Clientes', href: '/app/customers' },
          { label: 'Detalhe', href: `/app/customers/${params.id}` },
          { label: 'Artefatos' },
        ]}
        actions={
          <Button onClick={() => setShowCreate((value) => !value)}>
            {showCreate ? 'Cancelar' : 'Novo vínculo'}
          </Button>
        }
      />

      <FormFeedback error={!showCreate ? error : null} />

      {showCreate ? (
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
              <ArtifactCatalogPicker
                organizationId={organizationId}
                value={createForm.artifactId}
                onChange={(value) => setCreateForm({ artifactId: value })}
                required
              />
            </FormContainer>
          </CardContent>
        </Card>
      ) : null}

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
    </div>
  )
}
