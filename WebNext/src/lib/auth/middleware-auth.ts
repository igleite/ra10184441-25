import { type NextRequest, NextResponse } from 'next/server'

import { AUTH_TOKEN_COOKIE, resolveAuthCookieDomain } from '@/lib/auth/cookie'
import { normalizeHostname } from '@/lib/tenant'

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? ''

/** Rotas acessíveis sem sessão válida — não chamar validate-session aqui. */
export function isAuthPublicPath(pathname: string): boolean {
  if (pathname === '/' || pathname === '/register') {
    return true
  }

  if (pathname === '/403' || pathname === '/404' || pathname === '/no-access') {
    return true
  }

  if (pathname === '/auth' || pathname.startsWith('/auth/')) {
    return true
  }

  return false
}

export function requiresAuthenticatedSession(
  pathname: string,
  isTenantHost: boolean,
): boolean {
  if (isAuthPublicPath(pathname)) {
    return false
  }

  if (isTenantHost) {
    return pathname.startsWith('/app') || pathname.startsWith('/portal')
  }

  return pathname.startsWith('/admin') || pathname.startsWith('/account')
}

export async function isSessionValid(token: string): Promise<boolean> {
  if (!API_URL) {
    return true
  }

  try {
    const response = await fetch(`${API_URL}/api/auth/validate-session`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      cache: 'no-store',
    })

    return response.ok
  } catch {
    return false
  }
}

export function getRequestAuthToken(request: NextRequest): string | null {
  const fromCookies = request.cookies.get(AUTH_TOKEN_COOKIE)?.value
  return fromCookies?.trim() ? fromCookies : null
}

export function redirectToAuth(
  request: NextRequest,
  options?: { clearCookie?: boolean },
): NextResponse {
  const response = NextResponse.redirect(new URL('/auth', request.url))

  if (options?.clearCookie) {
    const hostname = normalizeHostname(request.headers.get('host') ?? '')
    const domain = resolveAuthCookieDomain(hostname)

    response.cookies.set(AUTH_TOKEN_COOKIE, '', {
      path: '/',
      ...(domain ? { domain } : {}),
      maxAge: 0,
      sameSite: 'lax',
      secure: request.nextUrl.protocol === 'https:',
    })
  }

  return response
}

export async function enforceAuthenticatedSession(
  request: NextRequest,
): Promise<NextResponse | null> {
  const token = getRequestAuthToken(request)

  if (!token) {
    return redirectToAuth(request)
  }

  const valid = await isSessionValid(token)

  if (!valid) {
    return redirectToAuth(request, { clearCookie: true })
  }

  return null
}
