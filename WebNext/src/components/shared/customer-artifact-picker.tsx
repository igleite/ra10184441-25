'use client'

import { useEffect, useMemo, useState } from 'react'

import { SelectField } from '@/components/shared/select-field'
import { apiGet } from '@/lib/api/client'
import {
  artifactCatalogLabel,
  fetchOrganizationArtifactCatalog,
  type CatalogArtifactDto,
} from '@/lib/artifact-catalog'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'

interface CustomerArtifactDto {
  id: string
  artifactId: string
}

export interface CustomerArtifactPickerProps {
  organizationId: string
  customerId: string
  id?: string
  label?: string
  value: string
  onChange: (artifactId: string) => void
  required?: boolean
  disabled?: boolean
  error?: string | null
}

export function CustomerArtifactPicker({
  organizationId,
  customerId,
  id = 'artifactId',
  label = 'Artefato',
  value,
  onChange,
  required = false,
  disabled = false,
  error,
}: CustomerArtifactPickerProps) {
  const [customerArtifacts, setCustomerArtifacts] = useState<CustomerArtifactDto[]>([])
  const [catalog, setCatalog] = useState<CatalogArtifactDto[]>([])
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [loadError, setLoadError] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${customerId}`
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    let cancelled = false

    async function load() {
      try {
        const [artifactsPage, catalogArtifacts] = await Promise.all([
          apiGet<PageDto<CustomerArtifactDto>>(
            `api/organizations/${organizationId}/customers/${customerId}/artifacts?pageIndex=1&pageSize=100`,
          ),
          fetchOrganizationArtifactCatalog(organizationId),
        ])

        if (!cancelled) {
          setCustomerArtifacts(artifactsPage.items ?? [])
          setCatalog(catalogArtifacts)
          setLoadError(null)
          setLoadedKey(fetchKey)
        }
      } catch (err) {
        if (!cancelled) {
          setCustomerArtifacts([])
          setCatalog([])
          setLoadError(formatApiErrorMessage(err as ApiError, 'Erro ao carregar artefatos do cliente.'))
          setLoadedKey(fetchKey)
        }
      }
    }

    void load()

    return () => {
      cancelled = true
    }
  }, [customerId, fetchKey, organizationId])

  const labelByArtifactId = useMemo(
    () => new Map(catalog.map((artifact) => [artifact.id, artifactCatalogLabel(artifact)])),
    [catalog],
  )

  const options = useMemo(
    () =>
      customerArtifacts.map((link) => ({
        value: link.artifactId,
        label: labelByArtifactId.get(link.artifactId) ?? link.artifactId,
      })),
    [customerArtifacts, labelByArtifactId],
  )

  return (
    <SelectField
      id={id}
      label={label}
      value={value}
      onChange={onChange}
      options={options}
      placeholder={
        isLoading
          ? 'Carregando artefatos...'
          : options.length === 0
            ? 'Nenhum artefato vinculado ao cliente'
            : 'Selecione o artefato...'
      }
      required={required}
      disabled={disabled || isLoading || options.length === 0}
      error={error ?? loadError}
      hint={
        options.length === 0 && !isLoading
          ? 'Vincule artefatos ao cliente em Clientes → Artefatos antes de abrir o ticket.'
          : undefined
      }
    />
  )
}
