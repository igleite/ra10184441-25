'use client'

import { useEffect, useMemo, useState } from 'react'

import { SelectField } from '@/components/shared/select-field'
import {
  artifactCatalogLabel,
  fetchOrganizationArtifactCatalog,
  type CatalogArtifactDto,
} from '@/lib/artifact-catalog'
import { formatApiErrorMessage, type ApiError } from '@/lib/api/types'

export interface ArtifactCatalogPickerProps {
  organizationId: string
  id?: string
  label?: string
  value: string
  onChange: (artifactId: string) => void
  required?: boolean
  disabled?: boolean
  error?: string | null
}

export function ArtifactCatalogPicker({
  organizationId,
  id = 'artifactId',
  label = 'Artefato',
  value,
  onChange,
  required = false,
  disabled = false,
  error,
}: ArtifactCatalogPickerProps) {
  const [catalog, setCatalog] = useState<CatalogArtifactDto[]>([])
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [loadError, setLoadError] = useState<string | null>(null)
  const isLoading = loadedKey !== organizationId

  useEffect(() => {
    let cancelled = false

    void fetchOrganizationArtifactCatalog(organizationId)
      .then((artifacts) => {
        if (!cancelled) {
          setCatalog(artifacts)
          setLoadError(null)
          setLoadedKey(organizationId)
        }
      })
      .catch((err: ApiError) => {
        if (!cancelled) {
          setCatalog([])
          setLoadError(formatApiErrorMessage(err, 'Erro ao carregar catálogo de artefatos.'))
          setLoadedKey(organizationId)
        }
      })

    return () => {
      cancelled = true
    }
  }, [organizationId])

  const options = useMemo(
    () =>
      catalog.map((artifact) => ({
        value: artifact.id,
        label: artifactCatalogLabel(artifact),
      })),
    [catalog],
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
            ? 'Nenhum artefato no catálogo'
            : 'Selecione o artefato...'
      }
      required={required}
      disabled={disabled || isLoading || options.length === 0}
      error={error ?? loadError}
      hint="Selecione um artefato do catálogo da organização."
    />
  )
}
