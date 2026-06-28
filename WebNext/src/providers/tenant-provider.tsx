'use client'

import { usePathname, useRouter } from 'next/navigation'
import {
  createContext,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from 'react'

import { apiGet } from '@/lib/api/client'
import type { ApiError } from '@/lib/api/types'
import { refreshSessionForTenant } from '@/lib/auth/session'
import { buildRootUrl, resolveTenantSlug } from '@/lib/tenant'
import {
  hasTenantOrgAccess,
  requiresTenantAccessCheck,
} from '@/lib/tenant/access'
import type { OrganizationDto } from '@/types/auth'
import { ROLES } from '@/types/auth'

interface TenantContextValue {
  slug: string
  organizationId: string
  organization: OrganizationDto | null
  isLoading: boolean
}

const TenantContext = createContext<TenantContextValue | null>(null)

function isPublicTenantPath(pathname: string): boolean {
  return (
    pathname === '/auth' ||
    pathname.startsWith('/auth/') ||
    pathname === '/no-access' ||
    pathname === '/403' ||
    pathname === '/404'
  )
}

function TenantLoading() {
  return (
    <div className="flex min-h-svh items-center justify-center text-sm text-muted-foreground">
      Carregando organização...
    </div>
  )
}

export function TenantProvider({ children }: { children: ReactNode }) {
  const router = useRouter()
  const pathname = usePathname()
  const [slug, setSlug] = useState<string | null>(null)
  const [slugReady, setSlugReady] = useState(false)

  const [organization, setOrganization] = useState<OrganizationDto | null>(null)
  const [accessReady, setAccessReady] = useState(!requiresTenantAccessCheck(pathname))
  const [isLoading, setIsLoading] = useState(true)

  const needsAccessCheck = requiresTenantAccessCheck(pathname)
  const isPublicPath = isPublicTenantPath(pathname)

  useEffect(() => {
    const resolvedSlug = resolveTenantSlug(window.location.host, pathname)
    setSlug(resolvedSlug)
    setSlugReady(true)

    if (!resolvedSlug) {
      router.replace('/403')
    }
  }, [pathname, router])

  useEffect(() => {
    if (!slug) {
      return
    }

    let cancelled = false
    setIsLoading(true)
    setAccessReady(!needsAccessCheck)

    async function loadOrganization() {
      try {
        const org = await apiGet<OrganizationDto>(
          `api/organizations/slug/${encodeURIComponent(slug!)}`,
        )

        if (cancelled) {
          return
        }

        setOrganization(org)

        if (!needsAccessCheck) {
          return
        }

        const user = await refreshSessionForTenant()
        if (cancelled) {
          return
        }

        if (!user) {
          router.replace('/auth')
          return
        }

        if (!hasTenantOrgAccess(user, org.id)) {
          // PendingOwner (Onboarding): somente rotas de conta na raiz.
          if (user.roles.includes(ROLES.onboarding)) {
            window.location.assign(
              buildRootUrl('/account/organizations/new', window.location.protocol),
            )
            return
          }

          router.replace(`/no-access?slug=${encodeURIComponent(slug!)}`)
          return
        }

        setAccessReady(true)
      } catch (err) {
        if (cancelled) {
          return
        }

        const apiError = err as ApiError
        if (apiError.statusCode === 401) {
          router.replace('/auth')
          return
        }

        router.replace('/404?reason=invalid-slug')
      } finally {
        if (!cancelled) {
          setIsLoading(false)
        }
      }
    }

    void loadOrganization()

    return () => {
      cancelled = true
    }
  }, [needsAccessCheck, router, slug])

  const value = useMemo<TenantContextValue | null>(() => {
    if (!slug) {
      return null
    }

    return {
      slug,
      organizationId: organization?.id ?? '',
      organization,
      isLoading,
    }
  }, [slug, organization, isLoading])

  if (!slugReady || !slug) {
    return <TenantLoading />
  }

  if (isPublicPath) {
    return (
      <TenantContext.Provider value={value}>{children}</TenantContext.Provider>
    )
  }

  if (isLoading || !accessReady || !organization || !value?.organizationId) {
    return <TenantLoading />
  }

  return (
    <TenantContext.Provider value={value}>{children}</TenantContext.Provider>
  )
}

export function useTenant(): TenantContextValue {
  const context = useContext(TenantContext)
  if (!context) {
    throw new Error('useTenant deve ser usado dentro de TenantProvider')
  }
  return context
}
