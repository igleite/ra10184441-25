'use client'

import { usePathname } from 'next/navigation'
import { useEffect, useState } from 'react'

import { apiGet } from '@/lib/api/client'
import { resolveTenantSlug } from '@/lib/tenant'
import type { OrganizationDto } from '@/types/auth'

export function useOrganizationBySlug() {
  const pathname = usePathname()
  const slug =
    typeof window !== 'undefined'
      ? resolveTenantSlug(window.location.host, pathname)
      : null
  const [organization, setOrganization] = useState<OrganizationDto | null>(null)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const isLoading = Boolean(slug) && loadedKey !== slug

  useEffect(() => {
    if (!slug) {
      return
    }

    let cancelled = false

    void apiGet<OrganizationDto>(
      `api/organizations/slug/${encodeURIComponent(slug)}`,
    )
      .then((org) => {
        if (!cancelled) {
          setOrganization(org)
        }
      })
      .catch(() => {
        if (!cancelled) {
          setOrganization(null)
        }
      })
      .finally(() => {
        if (!cancelled) {
          setLoadedKey(slug)
        }
      })

    return () => {
      cancelled = true
    }
  }, [slug])

  return {
    slug,
    organization: slug ? organization : null,
    isLoading,
  }
}
