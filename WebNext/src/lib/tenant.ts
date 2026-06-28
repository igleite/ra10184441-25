function parseRootDomain(raw: string): { hostname: string; port: string } {
  const trimmed = raw.trim()
  if (!trimmed) {
    return { hostname: 'localhost', port: '' }
  }

  const withPort = trimmed.match(/^([^:]+):(\d+)$/)
  if (withPort) {
    return {
      hostname: withPort[1].toLowerCase(),
      port: withPort[2],
    }
  }

  return { hostname: normalizeHostname(trimmed), port: '' }
}

const { hostname: ROOT_DOMAIN, port: ROOT_PORT } = parseRootDomain(
  process.env.NEXT_PUBLIC_ROOT_DOMAIN ?? 'localhost',
)
const DEV_ORG_SLUG = process.env.NEXT_PUBLIC_ORG_SLUG?.trim() ?? ''

export const LOCALTEST_ME = 'localtest.me'

export function normalizeHostname(host: string): string {
  return host.split(':')[0]?.toLowerCase() ?? ''
}

export function isLocalhostRoot(): boolean {
  return ROOT_DOMAIN === 'localhost' || ROOT_DOMAIN === '127.0.0.1'
}

export function isLocaltestRoot(): boolean {
  return ROOT_DOMAIN === LOCALTEST_ME
}

/** Dev com cookie compartilhado entre subdomínios (localhost, localtest.me ou *.test). */
export function usesSharedDevCookie(): boolean {
  return isLocalhostRoot() || isLocaltestRoot() || ROOT_DOMAIN.endsWith('.test')
}

/** Sufixo `:porta` para URLs em localhost (opcional; vem do env ou do browser). */
function resolveLocalhostPortSuffix(): string {
  if (typeof window !== 'undefined' && window.location.port) {
    const hostname = normalizeHostname(window.location.hostname)
    if (
      hostname === 'localhost' ||
      hostname === '127.0.0.1' ||
      hostname.endsWith('.localhost')
    ) {
      return `:${window.location.port}`
    }
  }

  return ROOT_PORT ? `:${ROOT_PORT}` : ''
}

export function isRootHostname(hostname: string): boolean {
  const normalized = normalizeHostname(hostname)
  return normalized === 'localhost' || normalized === ROOT_DOMAIN
}

export function isTenantScopedPath(pathname: string): boolean {
  return (
    pathname.startsWith('/app') ||
    pathname.startsWith('/portal') ||
    pathname === '/no-access'
  )
}

function isPlainRootHost(host: string): boolean {
  const hostname = normalizeHostname(host)
  return !hostname || isRootHostname(hostname)
}

export function getSubdomain(host: string): string | null {
  const hostname = normalizeHostname(host)

  if (!hostname || isRootHostname(hostname)) {
    return null
  }

  if (hostname.endsWith(`.${ROOT_DOMAIN}`)) {
    const slug = hostname.slice(0, -(ROOT_DOMAIN.length + 1))
    if (slug && !slug.includes('.')) {
      return slug
    }
  }

  if (hostname.endsWith('.localhost')) {
    const slug = hostname.slice(0, -'.localhost'.length)
    if (slug && !slug.includes('.')) {
      return slug
    }
  }

  if (hostname.endsWith(`.${LOCALTEST_ME}`)) {
    const slug = hostname.slice(0, -(LOCALTEST_ME.length + 1))
    if (slug && !slug.includes('.')) {
      return slug
    }
  }

  return null
}

export function buildTenantUrl(slug: string, path: string): string {
  const normalizedPath = path.startsWith('/') ? path : `/${path}`
  const protocol =
    typeof window !== 'undefined' ? window.location.protocol : 'http:'

  if (isLocalhostRoot()) {
    return `${protocol}//${slug}.localhost${resolveLocalhostPortSuffix()}${normalizedPath}`
  }

  return `${protocol}//${slug}.${ROOT_DOMAIN}${normalizedPath}`
}

/** URL absoluta apontando para subdomínio de tenant (não raiz). */
export function isTenantAbsoluteUrl(url: string): boolean {
  try {
    const parsed = new URL(url)

    if (parsed.protocol !== 'http:' && parsed.protocol !== 'https:') {
      return false
    }

    return getSubdomain(parsed.host) !== null
  } catch {
    return false
  }
}

export function buildRootUrl(path: string, protocol = 'http:'): string {
  const normalizedPath = path.startsWith('/') ? path : `/${path}`

  if (isLocalhostRoot()) {
    return `${protocol}//localhost${resolveLocalhostPortSuffix()}${normalizedPath}`
  }

  return `${protocol}//${ROOT_DOMAIN}${normalizedPath}`
}

export function isRootDomain(host: string): boolean {
  return getSubdomain(host) === null
}

/** Slug efetivo do tenant: subdomínio do host ou fallback de dev em localhost. */
export function resolveTenantSlug(
  host: string,
  pathname?: string,
): string | null {
  const fromHost = getSubdomain(host)
  if (fromHost) {
    return fromHost
  }

  if (!DEV_ORG_SLUG || !isPlainRootHost(host)) {
    return null
  }

  if (!pathname) {
    return DEV_ORG_SLUG
  }

  return isTenantScopedPath(pathname) ? DEV_ORG_SLUG : null
}

export { ROOT_DOMAIN, ROOT_PORT, DEV_ORG_SLUG }
