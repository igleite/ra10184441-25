'use client'

import { useEffect } from 'react'
import { usePathname, useRouter } from 'next/navigation'

import { usePermissions } from '@/hooks/use-permissions'
import { useAuth } from '@/providers/auth-provider'
import type { PermissionAction } from '@/lib/permissions'

interface UseRequirePermissionOptions {
  action?: PermissionAction
  appRoute?: boolean
  portalRoute?: boolean
  adminRoute?: boolean
  enabled?: boolean
}

export function useRequirePermission({
  action,
  appRoute = false,
  portalRoute = false,
  adminRoute = false,
  enabled = true,
}: UseRequirePermissionOptions) {
  const router = useRouter()
  const pathname = usePathname()
  const { isLoading } = useAuth()
  const { can, canAccessAdminRoute, canAccessAppRoute, canAccessPortalRoute } =
    usePermissions()

  useEffect(() => {
    if (!enabled || isLoading) {
      return
    }

    if (action && !can(action)) {
      router.replace('/403')
      return
    }

    if (appRoute && !canAccessAppRoute(pathname)) {
      router.replace('/403')
      return
    }

    if (portalRoute && !canAccessPortalRoute(pathname)) {
      router.replace('/403')
      return
    }

    if (adminRoute && !canAccessAdminRoute(pathname)) {
      router.replace('/403')
    }
  }, [
    action,
    adminRoute,
    appRoute,
    can,
    canAccessAdminRoute,
    canAccessAppRoute,
    canAccessPortalRoute,
    enabled,
    isLoading,
    pathname,
    portalRoute,
    router,
  ])
}
