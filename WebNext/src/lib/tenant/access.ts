import type { AuthUser } from '@/types/auth'
import { ROLES } from '@/types/auth'

const TENANT_ROLES = [
  ROLES.organizationOwner,
  ROLES.organizationMember,
  ROLES.clientAdmin,
  ROLES.clientMember,
] as const

export function hasTenantOrgAccess(
  user: AuthUser,
  organizationId: string,
): boolean {
  if (user.roles.includes(ROLES.superAdmin)) {
    return true
  }

  const hasTenantRole = user.roles.some((role) =>
    TENANT_ROLES.includes(role as (typeof TENANT_ROLES)[number]),
  )

  if (!hasTenantRole) {
    return false
  }

  if (user.organizationId && user.organizationId !== organizationId) {
    return false
  }

  return true
}

export function requiresTenantAccessCheck(pathname: string): boolean {
  return pathname.startsWith('/app') || pathname.startsWith('/portal')
}
