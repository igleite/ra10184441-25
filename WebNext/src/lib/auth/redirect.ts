import { fetchMyOrganizations } from '@/lib/account-organizations'
import {
  clearPendingReturnUrl,
  getPendingReturnUrl,
} from '@/lib/auth/pending-return-url'
import {
  isReturnUrlValidForUser,
  resolveReturnUrl,
} from '@/lib/auth/return-url'
import { buildAuthUserFromLogin } from '@/lib/auth/user'
import { normalizeLoginResponse } from '@/lib/auth/login-response'
import { buildTenantUrl } from '@/lib/tenant'
import type { AuthUser, LoginResponse } from '@/types/auth'
import { ROLES } from '@/types/auth'

async function resolveTenantDestination(
  user: AuthUser,
  token: string,
  path: '/app' | '/portal',
): Promise<string> {
  try {
    const mine = await fetchMyOrganizations(user.userId, token)
    if (mine.length > 0) {
      return buildTenantUrl(mine[0].slug, path)
    }
  } catch {
    // fallback abaixo
  }

  return '/auth'
}

async function resolveDefaultDestination(
  user: AuthUser,
  token: string,
): Promise<string> {
  if (user.roles.includes(ROLES.superAdmin)) {
    return '/admin'
  }

  if (user.roles.includes(ROLES.onboarding)) {
    return '/account/organizations/new'
  }

  if (user.roles.includes(ROLES.organizationOwner)) {
    return '/app'
  }

  if (user.roles.includes(ROLES.organizationMember)) {
    return resolveTenantDestination(user, token, '/app')
  }

  if (user.roles.includes(ROLES.clientAdmin)) {
    return resolveTenantDestination(user, token, '/portal')
  }

  if (user.roles.includes(ROLES.clientMember)) {
    return resolveTenantDestination(user, token, '/portal')
  }

  return '/auth'
}

export async function resolvePostAuthRedirect(
  login: LoginResponse,
  returnUrl?: string | null,
): Promise<string> {
  const normalized = normalizeLoginResponse(login)
  const user = buildAuthUserFromLogin(normalized)
  const { token } = normalized

  const explicitReturn = resolveReturnUrl(returnUrl)
  if (explicitReturn && isReturnUrlValidForUser(explicitReturn, user)) {
    clearPendingReturnUrl()
    return explicitReturn
  }

  const defaultDestination = await resolveDefaultDestination(user, token)

  const pendingReturnUrl = getPendingReturnUrl()
  if (
    pendingReturnUrl &&
    isReturnUrlValidForUser(pendingReturnUrl, user)
  ) {
    clearPendingReturnUrl()
    return pendingReturnUrl
  }

  clearPendingReturnUrl()
  return defaultDestination
}

export function isAbsoluteUrl(path: string): boolean {
  return /^https?:\/\//i.test(path)
}
