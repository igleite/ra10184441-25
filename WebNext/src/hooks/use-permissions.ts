'use client'

import { useMemo } from 'react'

import {
  canAccessAdminRoute,
  canAccessAppRoute,
  canAccessPortalRoute,
  filterAdminNavItems,
  filterAppNavItems,
  filterPortalNavItems,
  getPrimaryAppRole,
  getPrimaryPortalRole,
  hasPermission,
  type PermissionAction,
} from '@/lib/permissions'
import { useAuth } from '@/providers/auth-provider'
import type { RoleName } from '@/types/auth'

export function usePermissions() {
  const { user, hasRole, hasAnyRole } = useAuth()

  return useMemo(() => {
    const roles = user?.roles ?? []

    return {
      roles,
      hasRole,
      hasAnyRole,
      can: (action: PermissionAction) => hasPermission(roles, action),
      canAccessAppRoute: (pathname: string) => canAccessAppRoute(pathname, roles),
      canAccessPortalRoute: (pathname: string) =>
        canAccessPortalRoute(pathname, roles),
      canAccessAdminRoute: (pathname: string) => canAccessAdminRoute(pathname, roles),
      appNavItems: filterAppNavItems(roles),
      portalNavItems: filterPortalNavItems(roles),
      adminNavItems: filterAdminNavItems(roles),
      primaryAppRole: getPrimaryAppRole(roles),
      primaryPortalRole: getPrimaryPortalRole(roles),
      isAppManagement: hasPermission(roles, 'catalog:manage'),
      isPortalAdmin: hasPermission(roles, 'portal-users:manage'),
    }
  }, [hasAnyRole, hasRole, user?.roles])
}

export type UsePermissionsResult = ReturnType<typeof usePermissions>

export function useRoleLabel(role: RoleName | null): string {
  const labels: Record<RoleName, string> = {
    SuperAdmin: 'Super Admin',
    Onboarding: 'Aguardando organização',
    OrganizationOwner: 'Dono da organização',
    OrganizationMember: 'Membro da organização',
    ClientAdmin: 'Admin do cliente',
    ClientMember: 'Membro do cliente',
    Nullable: 'Sem papel',
  }
  return role ? labels[role] : ''
}
