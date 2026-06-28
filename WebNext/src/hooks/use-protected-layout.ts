'use client'

import { useRouter } from 'next/navigation'
import { useEffect } from 'react'

import { useAuth } from '@/providers/auth-provider'
import { buildRootUrl, resolveTenantSlug } from '@/lib/tenant'

interface UseProtectedLayoutOptions {
  isAllowed: boolean
  authRedirect?: string
  deniedRedirect?: string
}

function navigateToRedirect(
  router: ReturnType<typeof useRouter>,
  target: string,
): void {
  if (/^https?:\/\//i.test(target)) {
    window.location.assign(target)
    return
  }

  const slug = resolveTenantSlug(window.location.host, window.location.pathname)
  if (
    slug &&
    (target.startsWith('/account') || target.startsWith('/admin'))
  ) {
    window.location.assign(buildRootUrl(target, window.location.protocol))
    return
  }

  router.replace(target)
}

export function useProtectedLayout({
  isAllowed,
  authRedirect = '/auth',
  deniedRedirect = '/403',
}: UseProtectedLayoutOptions) {
  const router = useRouter()
  const { isAuthenticated, isLoading, user } = useAuth()

  useEffect(() => {
    if (isLoading) {
      return
    }

    console.log('useProtectedLayout', isAuthenticated, user?.roles.length, isAllowed)

    if (!isAuthenticated) {
      router.replace(authRedirect)
      return
    }

    // if (!user?.roles.length) {
    //   navigateToRedirect(router, deniedRedirect)
    //   return
    // }

    if (!isAllowed) {
      navigateToRedirect(router, deniedRedirect)
    }
  }, [
    authRedirect,
    deniedRedirect,
    isAllowed,
    isAuthenticated,
    isLoading,
    router,
    user,
  ])

  return {
    /** validate-session em andamento no client */
    isBootstrapping: isLoading,
    /** Sessão restaurada; aguardando redirect de permissão ou rota */
    isCheckingAccess:
      !isLoading &&
      (!isAuthenticated || !user?.roles.length || !isAllowed),
    isReady:
      !isLoading && isAuthenticated && Boolean(user?.roles.length) && isAllowed,
  }
}
