import { apiGet } from '@/lib/api/client'
import {
  clearAuthCookie,
  getAuthTokenFromCookie,
  setAuthCookie,
} from '@/lib/auth/cookie'
import { buildAuthUserFromLogin, buildAuthUserFromSessionUser } from '@/lib/auth/user'
import {
  normalizeLoginResponse,
  normalizeSessionUserDto,
} from '@/lib/auth/login-response'
import type { AuthUser, LoginResponse, SessionUserDto } from '@/types/auth'

export interface SessionSnapshot {
  token: string | null
  user: AuthUser | null
  isLoading: boolean
}

const SERVER_SNAPSHOT: SessionSnapshot = {
  token: null,
  user: null,
  isLoading: true,
}

let snapshot: SessionSnapshot = {
  token: null,
  user: null,
  isLoading: true,
}

let hasBootstrapped = false
let bootstrapPromise: Promise<void> | null = null
let sessionEpoch = 0

function notifySessionChange() {
  if (typeof window === 'undefined') {
    return
  }

  window.dispatchEvent(new Event('auth-session-change'))
}

let cachedClientSnapshot: SessionSnapshot = snapshot

function commitSnapshot(next: SessionSnapshot) {
  snapshot = next
  cachedClientSnapshot = next
  notifySessionChange()
}

function readSnapshot(): SessionSnapshot {
  const token = getAuthTokenFromCookie() ?? snapshot.token
  const { user, isLoading } = snapshot

  if (
    cachedClientSnapshot.token === token &&
    cachedClientSnapshot.user === user &&
    cachedClientSnapshot.isLoading === isLoading
  ) {
    return cachedClientSnapshot
  }

  cachedClientSnapshot = { token, user, isLoading }
  return cachedClientSnapshot
}

function isStaleBootstrap(epoch: number, bootstrapToken: string | null): boolean {
  if (epoch !== sessionEpoch) {
    return true
  }

  return getAuthTokenFromCookie() !== bootstrapToken
}

function snapshotNeedsHydration(cookieToken: string | null): boolean {
  if (!cookieToken) {
    return Boolean(snapshot.token || snapshot.user)
  }

  if (!snapshot.user || snapshot.token !== cookieToken) {
    return true
  }

  return false
}

function settleBootstrap(epoch: number, next: SessionSnapshot) {
  if (isStaleBootstrap(epoch, next.token)) {
    const cookieToken = getAuthTokenFromCookie()
    if (snapshot.user && cookieToken === snapshot.token) {
      commitSnapshot({ ...snapshot, isLoading: false })
    } else if (snapshot.isLoading && !cookieToken) {
      commitSnapshot({ token: null, user: null, isLoading: false })
    }
    return
  }

  commitSnapshot(next)
}

async function bootstrapSession(): Promise<void> {
  const epoch = sessionEpoch
  const bootstrapToken = getAuthTokenFromCookie()

  commitSnapshot({
    token: bootstrapToken ?? snapshot.token,
    user: snapshot.user,
    isLoading: true,
  })

  if (!bootstrapToken) {
    settleBootstrap(epoch, { token: null, user: null, isLoading: false })
    return
  }

  try {
    const sessionUser = normalizeSessionUserDto(
      await apiGet<SessionUserDto>('api/auth/validate-session', bootstrapToken),
    )

    settleBootstrap(epoch, {
      token: bootstrapToken,
      user: buildAuthUserFromSessionUser(sessionUser, bootstrapToken),
      isLoading: false,
    })
  } catch {
    settleBootstrap(epoch, { token: null, user: null, isLoading: false })
    if (!isStaleBootstrap(epoch, bootstrapToken)) {
      clearAuthCookie()
    }
  }
}

function shouldRunBootstrap(): boolean {
  const cookieToken = getAuthTokenFromCookie()

  if (!hasBootstrapped) {
    return true
  }

  return snapshotNeedsHydration(cookieToken)
}

export function ensureSessionBootstrapped(): Promise<void> {
  if (typeof window === 'undefined') {
    return Promise.resolve()
  }

  if (!shouldRunBootstrap()) {
    if (snapshot.isLoading && snapshot.user) {
      commitSnapshot({ ...snapshot, isLoading: false })
    }
    return Promise.resolve()
  }

  if (!bootstrapPromise) {
    bootstrapPromise = bootstrapSession()
      .then(() => {
        hasBootstrapped = true
      })
      .finally(() => {
        bootstrapPromise = null
      })
  }

  return bootstrapPromise
}

/** Força nova leitura do cookie + validate-session (ex.: após verify, antes de navegar). */
export function rebootstrapSession(): Promise<void> {
  if (typeof window === 'undefined') {
    return Promise.resolve()
  }

  sessionEpoch += 1
  hasBootstrapped = false
  bootstrapPromise = null
  return ensureSessionBootstrapped()
}

export function establishSession(login: LoginResponse): void {
  const normalized = normalizeLoginResponse(login)
  const token = normalized.token

  if (!token) {
    throw new Error('Resposta de login sem token.')
  }

  sessionEpoch += 1
  setAuthCookie(token, normalized.expiresAt)

  if (getAuthTokenFromCookie() !== token) {
    throw new Error('Não foi possível gravar o token de sessão no navegador.')
  }

  const user = buildAuthUserFromLogin(normalized)

  commitSnapshot({
    token,
    user,
    isLoading: false,
  })

  // Evita bootstrap concorrente na página de verify; a próxima carga fará hydrate.
  hasBootstrapped = true
  bootstrapPromise = null
}

export async function clearSession(): Promise<void> {
  sessionEpoch += 1
  clearAuthCookie()
  hasBootstrapped = true
  bootstrapPromise = null

  commitSnapshot({ token: null, user: null, isLoading: false })
}

/** Revalida sessão com Origin do subdomínio atual (roles tenant-aware na Api). */
export async function refreshSessionForTenant(): Promise<AuthUser | null> {
  const epoch = sessionEpoch
  const token = getAuthTokenFromCookie()

  if (!token) {
    commitSnapshot({ token: null, user: null, isLoading: false })
    hasBootstrapped = true
    return null
  }

  commitSnapshot({ ...snapshot, isLoading: true })

  try {
    const sessionUser = normalizeSessionUserDto(
      await apiGet<SessionUserDto>('api/auth/validate-session', token),
    )

    if (isStaleBootstrap(epoch, token)) {
      return snapshot.user
    }

    const user = buildAuthUserFromSessionUser(sessionUser, token)
    hasBootstrapped = true
    commitSnapshot({ token, user, isLoading: false })
    return user
  } catch {
    if (isStaleBootstrap(epoch, token)) {
      return snapshot.user
    }

    clearAuthCookie()
    hasBootstrapped = true
    commitSnapshot({ token: null, user: null, isLoading: false })
    return null
  }
}

export function subscribeToSession(callback: () => void): () => void {
  const onChange = () => callback()

  window.addEventListener('auth-session-change', onChange)
  void ensureSessionBootstrapped()

  return () => {
    window.removeEventListener('auth-session-change', onChange)
  }
}

export function getClientSessionSnapshot(): SessionSnapshot {
  void ensureSessionBootstrapped()
  return readSnapshot()
}

export function getServerSessionSnapshot(): SessionSnapshot {
  return SERVER_SNAPSHOT
}
