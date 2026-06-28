import type { NextRequest } from 'next/server'
import { NextResponse } from 'next/server'

import {
  enforceAuthenticatedSession,
  requiresAuthenticatedSession,
} from '@/lib/auth/middleware-auth'
import { resolveTenantSlug } from '@/lib/tenant'

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? ''

async function isValidOrganizationSlug(slug: string): Promise<boolean> {
  if (!API_URL) {
    return true
  }

  try {
    const response = await fetch(
      `${API_URL}/api/organizations/slug/${encodeURIComponent(slug)}`,
      { cache: 'no-store' },
    )

    return response.ok
  } catch {
    return false
  }
}

export async function middleware(request: NextRequest) {
  console.log('middleware', request.url)
  
  const { pathname } = request.nextUrl
  const host = request.headers.get('host') ?? ''
  const slug = resolveTenantSlug(host, pathname)
  const isTenantHost = slug !== null

  if (isTenantHost) {
    if (pathname === '/' || pathname === '/register') {
      return NextResponse.redirect(new URL('/auth', request.url))
    }

    if (pathname === '/auth/forgot-password' || pathname === '/auth/forgot') {
      return NextResponse.redirect(new URL('/auth', request.url))
    }

    if (
      pathname.startsWith('/admin') ||
      pathname.startsWith('/account')
    ) {
      return NextResponse.redirect(new URL('/403?context=tenant', request.url))
    }

    const valid = await isValidOrganizationSlug(slug)
    if (!valid) {
      const url = request.nextUrl.clone()
      url.pathname = '/404'
      url.searchParams.set('reason', 'invalid-slug')
      return NextResponse.rewrite(url)
    }
  } else {
    if (pathname === '/auth/forgot-password' || pathname === '/auth/forgot') {
      return NextResponse.redirect(new URL('/auth', request.url))
    }

    if (pathname.startsWith('/app') || pathname.startsWith('/portal')) {
      return NextResponse.redirect(new URL('/403', request.url))
    }
  }

  if (requiresAuthenticatedSession(pathname, isTenantHost)) {
    const authResponse = await enforceAuthenticatedSession(request)
    if (authResponse) {
      return authResponse
    }
  }

  return NextResponse.next()
}

export const config = {
  matcher: [
    /*
     * Executa em todas as rotas exceto assets estáticos.
     * Rotas públicas de auth (/auth, /auth/verify, /register, /403, /404)
     * passam pelo middleware para rewrites de tenant/raiz, mas ficam fora
     * do guard validate-session via isAuthPublicPath().
     */
    '/((?!_next/static|_next/image|favicon.ico|.*\\.(?:svg|png|jpg|jpeg|gif|webp|ico)$).*)',
  ],
}

export { ROOT_DOMAIN } from '@/lib/tenant'
