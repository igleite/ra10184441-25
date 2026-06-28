import {
  isLocalhostRoot,
  isLocaltestRoot,
  LOCALTEST_ME,
  normalizeHostname,
  ROOT_DOMAIN,
  usesSharedDevCookie,
} from '@/lib/tenant'

export const AUTH_TOKEN_COOKIE = 'auth_token'

/** Domínio do cookie para o host atual. `undefined` = cookie host-only (sem atributo domain). */
export function resolveAuthCookieDomain(hostname?: string): string | undefined {
  const normalized = normalizeHostname(hostname ?? '')

  if (!normalized || normalized === 'localhost' || normalized === '127.0.0.1') {
    return undefined
  }

  if (normalized.endsWith('.localtest.me')) {
    return `.${LOCALTEST_ME}`
  }

  if (normalized.endsWith('.localhost')) {
    return '.localhost'
  }

  if (isLocalhostRoot()) {
    return '.localhost'
  }

  if (isLocaltestRoot()) {
    return `.${LOCALTEST_ME}`
  }

  return `.${ROOT_DOMAIN}`
}

/** @deprecated Use resolveAuthCookieDomain — mantido para cookies legados em pending-return-url. */
export function getAuthCookieDomain(): string {
  return resolveAuthCookieDomain() ?? 'localhost'
}

function shouldUseSecureCookie(): boolean {
  if (typeof window !== 'undefined') {
    return window.location.protocol === 'https:'
  }

  return !usesSharedDevCookie()
}

export function parseAuthTokenFromCookieHeader(
  cookieHeader: string | null | undefined,
): string | null {
  if (!cookieHeader) {
    return null
  }

  const match = cookieHeader.match(
    new RegExp(`(?:^|;\\s*)${AUTH_TOKEN_COOKIE}=([^;]+)`),
  )
  const raw = match?.[1]

  if (!raw) {
    return null
  }

  try {
    return decodeURIComponent(raw)
  } catch {
    return raw
  }
}

export function getAuthTokenFromDocumentCookie(): string | null {
  if (typeof document === 'undefined') {
    return null
  }

  return parseAuthTokenFromCookieHeader(document.cookie)
}

function buildAuthCookieValue(
  token: string,
  expiresAt?: string | Date,
  domain?: string | null,
): string {
  const parts = [
    `${AUTH_TOKEN_COOKIE}=${encodeURIComponent(token)}`,
    'path=/',
    'SameSite=Lax',
  ]

  const resolvedDomain =
    domain === null
      ? undefined
      : domain === undefined
        ? resolveAuthCookieDomain(
            typeof window !== 'undefined' ? window.location.hostname : undefined,
          )
        : domain

  if (resolvedDomain) {
    parts.push(`domain=${resolvedDomain}`)
  }

  if (shouldUseSecureCookie()) {
    parts.push('Secure')
  }

  if (expiresAt) {
    const date = typeof expiresAt === 'string' ? new Date(expiresAt) : expiresAt
    if (!Number.isNaN(date.getTime())) {
      parts.push(`expires=${date.toUTCString()}`)
    }
  }

  return parts.join('; ')
}

function clearAuthCookieVariants(): void {
  if (typeof document === 'undefined') {
    return
  }

  const hostname =
    typeof window !== 'undefined' ? window.location.hostname : undefined
  const domains = new Set<string | undefined>([
    undefined,
    resolveAuthCookieDomain(hostname),
    '.localhost',
    `.${LOCALTEST_ME}`,
    ROOT_DOMAIN === 'localhost' ? undefined : `.${ROOT_DOMAIN}`,
  ])

  for (const domain of domains) {
    const parts = [
      `${AUTH_TOKEN_COOKIE}=`,
      'path=/',
      'max-age=0',
      'SameSite=Lax',
    ]

    if (domain) {
      parts.push(`domain=${domain}`)
    }

    if (shouldUseSecureCookie()) {
      parts.push('Secure')
    }

    document.cookie = parts.join('; ')
  }
}

export function setAuthCookie(token: string, expiresAt?: string | Date): void {
  if (typeof document === 'undefined') {
    return
  }

  const hostname = window.location.hostname

  if (isLocaltestRoot()) {
    document.cookie = buildAuthCookieValue(token, expiresAt, `.${LOCALTEST_ME}`)
    return
  }

  if (!isLocalhostRoot()) {
    const sharedDomain = resolveAuthCookieDomain(hostname)
    if (sharedDomain) {
      document.cookie = buildAuthCookieValue(token, expiresAt, sharedDomain)
      return
    }
  }

  // Em localhost sem subdomínio, host-only é o único modo confiável no Chrome.
  document.cookie = buildAuthCookieValue(token, expiresAt, null)

  if (getAuthTokenFromDocumentCookie() === token) {
    return
  }

  // Fallback: domínio compartilhado (.localhost) para subdomínios.
  const sharedDomain = resolveAuthCookieDomain(hostname) ?? '.localhost'
  document.cookie = buildAuthCookieValue(token, expiresAt, sharedDomain)

  if (getAuthTokenFromDocumentCookie() !== token) {
    document.cookie = buildAuthCookieValue(token, expiresAt, null)
  }
}

export function clearAuthCookie(): void {
  clearAuthCookieVariants()
}

/** Lê o token de autenticação exclusivamente do cookie compartilhado entre subdomínios. */
export function getAuthTokenFromCookie(): string | null {
  return getAuthTokenFromDocumentCookie()
}
