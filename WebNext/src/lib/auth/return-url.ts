import { isRootHostname, ROOT_DOMAIN } from '@/lib/tenant'
import type { AuthUser } from '@/types/auth'
import { ROLES } from '@/types/auth'

function isPendingOwnerRoute(pathname: string): boolean {
  return (
    pathname === '/account' ||
    pathname === '/account/profile' ||
    pathname === '/account/organizations' ||
    pathname === '/account/organizations/new' ||
    pathname.startsWith('/account/organizations/new')
  )
}

export function isAllowedReturnUrl(returnUrl: string): boolean {
  try {
    const url = new URL(returnUrl)
    const hostname = url.hostname.toLowerCase()

    if (isRootHostname(hostname)) {
      return true
    }

    if (hostname.endsWith('.localhost')) {
      return true
    }

    if (hostname.endsWith('.localtest.me')) {
      return true
    }

    if (ROOT_DOMAIN !== 'localhost' && hostname.endsWith(`.${ROOT_DOMAIN}`)) {
      return true
    }

    return false
  } catch {
    return false
  }
}

export function isReturnUrlValidForUser(
  returnUrl: string,
  user: AuthUser,
): boolean {
  if (!isAllowedReturnUrl(returnUrl)) {
    return false
  }

  const url = new URL(returnUrl)
  const pathname = url.pathname

  if (
    isRootHostname(url.hostname) &&
    (pathname.startsWith('/app') || pathname.startsWith('/portal'))
  ) {
    return false
  }

  if (user.roles.includes(ROLES.onboarding)) {
    return isRootHostname(url.hostname) && isPendingOwnerRoute(pathname)
  }

  if (user.roles.includes(ROLES.superAdmin)) {
    return true
  }

  if (isRootHostname(url.hostname) && pathname.startsWith('/account')) {
    return (
      user.roles.includes(ROLES.organizationOwner) ||
      user.roles.includes(ROLES.organizationMember)
    )
  }

  return true
}

export function resolveReturnUrl(
  returnUrl: string | null | undefined,
): string | null {
  if (!returnUrl || !isAllowedReturnUrl(returnUrl)) {
    return null
  }

  return returnUrl
}
