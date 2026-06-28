import { apiGet } from '@/lib/api/client'
import type { PageDto } from '@/lib/api/types'

export interface CatalogArtifactDto {
  id: string
  name: string
  code: string
}

interface ArtifactTypeDto {
  id: string
  name: string
}

const DEFAULT_PAGE_SIZE = 100

export function artifactCatalogLabel(artifact: CatalogArtifactDto): string {
  return `${artifact.name} (${artifact.code})`
}

export async function fetchOrganizationArtifactCatalog(
  organizationId: string,
  pageSize = DEFAULT_PAGE_SIZE,
): Promise<CatalogArtifactDto[]> {
  const typesPage = await apiGet<PageDto<ArtifactTypeDto>>(
    `api/organizations/${organizationId}/artifact-types?pageIndex=1&pageSize=${pageSize}`,
  )
  const allArtifacts: CatalogArtifactDto[] = []

  for (const type of typesPage.items ?? []) {
    const artifactsPage = await apiGet<PageDto<CatalogArtifactDto>>(
      `api/organizations/${organizationId}/artifact-types/${type.id}/artifacts?pageIndex=1&pageSize=${pageSize}`,
    ).catch(() => ({
      items: [],
      totalItemCount: 0,
      pageIndex: 1,
      pageSize,
    } satisfies PageDto<CatalogArtifactDto>))
    allArtifacts.push(...(artifactsPage.items ?? []))
  }

  return allArtifacts
}
