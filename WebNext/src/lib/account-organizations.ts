import { apiGet } from '@/lib/api/client'
import type { PageDto } from '@/lib/api/types'

export interface AccountOrganizationDto {
  id: string
  name: string
  document: string
  slug: string
}

interface OrganizationUserDto {
  userId: string
}

export async function filterMyOrganizations(
  organizations: AccountOrganizationDto[],
  userId: string,
): Promise<AccountOrganizationDto[]> {
  const results = await Promise.all(
    organizations.map(async (organization) => {
      try {
        const isMember = await isUserOrganizationMember(organization.id, userId)
        return isMember ? organization : null
      } catch {
        return null
      }
    }),
  )

  return results.filter(
    (organization): organization is AccountOrganizationDto => organization !== null,
  )
}

export async function isUserOrganizationMember(
  organizationId: string,
  userId: string,
): Promise<boolean> {
  const members = await apiGet<PageDto<OrganizationUserDto>>(
    `api/organizations/${organizationId}/users?pageIndex=1&pageSize=100`,
  )

  return members.items?.some((member) => member.userId === userId) ?? false
}

export async function fetchMyOrganizations(
  userId: string,
  token?: string,
): Promise<AccountOrganizationDto[]> {
  const page = await apiGet<PageDto<AccountOrganizationDto>>(
    'api/organizations?pageIndex=1&pageSize=100',
    token,
  )

  return filterMyOrganizations(page.items ?? [], userId)
}
