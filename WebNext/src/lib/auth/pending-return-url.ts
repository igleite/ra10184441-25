import { isAllowedReturnUrl } from '@/lib/auth/return-url'
import { resolveAuthCookieDomain } from '@/lib/auth/cookie'
import { usesSharedDevCookie } from '@/lib/tenant'

export const PENDING_RETURN_URL_COOKIE = 'auth_pending_return_url'

const MAX_AGE_SECONDS = 15 * 60

function shouldUseSecureCookie(): boolean {
  if (typeof window !== 'undefined') {
    return window.location.protocol === 'https:'
  }

  return !usesSharedDevCookie()
}

function buildCookieParts(value: string, maxAge: number): string[] {
  const parts = [
    `${PENDING_RETURN_URL_COOKIE}=${encodeURIComponent(value)}`,
    'path=/',
    'SameSite=Lax',
    `max-age=${maxAge}`,
  ]

  const domain = resolveAuthCookieDomain(
    typeof window !== 'undefined' ? window.location.hostname : undefined,
  )

  if (domain) {
    parts.splice(2, 0, `domain=${domain}`)
  }

  if (shouldUseSecureCookie()) {
    parts.push('Secure')
  }

  return parts
}

export function setPendingReturnUrl(returnUrl: string): void {
  if (typeof document === 'undefined' || !isAllowedReturnUrl(returnUrl)) {
    return
  }

  document.cookie = buildCookieParts(returnUrl, MAX_AGE_SECONDS).join('; ')
}

export function getPendingReturnUrl(): string | null {
  if (typeof document === 'undefined') {
    return null
  }

  const match = document.cookie.match(
    new RegExp(`(?:^|;\\s*)${PENDING_RETURN_URL_COOKIE}=([^;]+)`),
  )
  const raw = match?.[1]

  if (!raw) {
    return null
  }

  try {
    const value = decodeURIComponent(raw)
    return isAllowedReturnUrl(value) ? value : null
  } catch {
    return null
  }
}

export function clearPendingReturnUrl(): void {
  if (typeof document === 'undefined') {
    return
  }

  document.cookie = buildCookieParts('', 0).join('; ')
}
