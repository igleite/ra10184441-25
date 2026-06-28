'use client'

import { useParams, useRouter } from 'next/navigation'
import { useEffect, useMemo, useState } from 'react'

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
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
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

const CATALOG_PAGE_SIZE = 100

export default function PortalArtifactDetailPage() {
  const params = useParams<{ id: string }>()
  const router = useRouter()
  const { organizationId } = useTenant()
  const { customerId } = useCustomer()
  const { can } = usePermissions()
  const canManageArtifacts = can('portal-artifacts:manage')
  const [artifact, setArtifact] = useState<CustomerArtifactDto | null>(null)
  const [form, setForm] = useState({ artifactId: '' })
  const [catalogArtifacts, setCatalogArtifacts] = useState<CatalogArtifactDto[]>([])
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [showDelete, setShowDelete] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)

  const fetchKey = `${organizationId}-${customerId ?? 'none'}-${params.id}`
  const isLoading = customerId ? loadedKey !== fetchKey : false

  const catalogOptions = useMemo(
    () =>
      catalogArtifacts.map((item) => ({
        value: item.id,
        label: `${item.name} (${item.code})`,
      })),
    [catalogArtifacts],
  )

  const artifactDisplayLabel = useMemo(() => {
    const match = catalogArtifacts.find((item) => item.id === form.artifactId)
    return match ? `${match.name} (${match.code})` : form.artifactId
  }, [catalogArtifacts, form.artifactId])

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

  useEffect(() => {
    if (!customerId) {
      return
    }

    let cancelled = false

    void apiGet<CustomerArtifactDto>(
      `api/organizations/${organizationId}/customers/${customerId}/artifacts/${params.id}`,
    )
      .then((loaded) => {
        if (cancelled) {
          return
        }

        if (loaded.customerId !== customerId) {
          setArtifact(null)
          setError('Artefato não encontrado.')
          setLoadedKey(fetchKey)
          return
        }

        setArtifact(loaded)
        setForm({ artifactId: loaded.artifactId })
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        setError(formatApiErrorMessage(err, 'Artefato não encontrado.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [customerId, fetchKey, organizationId, params.id])

  async function handleSubmit() {
    if (!artifact || !customerId || !canManageArtifacts) {
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const updated = await apiPut<CustomerArtifactDto>(
        `api/organizations/${organizationId}/customers/${customerId}/artifacts/${params.id}`,
        {
          artifactId: form.artifactId.trim(),
          rowVersion: artifact.rowVersion,
        },
      )
      setArtifact(updated)
      setForm({ artifactId: updated.artifactId })
      router.push('/portal/artifacts')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar artefato.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete() {
    if (!artifact || !customerId || !canManageArtifacts) {
      return
    }

    setIsDeleting(true)
    setError(null)

    try {
      await apiDelete(
        `api/organizations/${organizationId}/customers/${customerId}/artifacts/${params.id}?rowVersion=${encodeURIComponent(artifact.rowVersion)}`,
      )
      router.push('/portal/artifacts')
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao excluir artefato.'))
    } finally {
      setIsDeleting(false)
      setShowDelete(false)
    }
  }

  if (!customerId) {
    return <FormFeedback error="Cliente não identificado no token." />
  }

  if (isLoading) {
    return <PageSkeleton fields={1} />
  }

  if (!artifact) {
    return <FormFeedback error={error ?? 'Artefato não encontrado.'} />
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title={artifactDisplayLabel}
        subtitle={
          canManageArtifacts
            ? 'Edição do vínculo de artefato do cliente.'
            : 'Visualização do artefato do cliente (somente leitura).'
        }
        breadcrumbs={[
          { label: 'Artefatos', href: '/portal/artifacts' },
          { label: artifact.id },
        ]}
        actions={
          <PermissionGate action="portal-artifacts:manage">
            <Button variant="destructive" size="sm" onClick={() => setShowDelete(true)}>
              Excluir
            </Button>
          </PermissionGate>
        }
      />

      {canManageArtifacts ? (
        <FormContainer
          onSubmit={handleSubmit}
          isSubmitting={isSubmitting}
          cancelHref="/portal/artifacts"
          error={error}
        >
          <SelectField
            id="artifactId"
            label="Artefato"
            value={form.artifactId}
            onChange={(value) => setForm({ artifactId: value })}
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
      ) : (
        <div className="space-y-2 rounded-md border p-4">
          <Label htmlFor="artifactId">Artefato</Label>
          <Input id="artifactId" value={artifactDisplayLabel} readOnly disabled />
        </div>
      )}

      <PermissionGate action="portal-artifacts:manage">
        <ConfirmDialog
          open={showDelete}
          onOpenChange={setShowDelete}
          title="Excluir artefato"
          description={`Confirma a exclusão do vínculo "${artifact.id}"?`}
          confirmLabel={isDeleting ? 'Excluindo...' : 'Excluir'}
          variant="destructive"
          onConfirm={handleDelete}
        />
      </PermissionGate>
    </div>
  )
}
