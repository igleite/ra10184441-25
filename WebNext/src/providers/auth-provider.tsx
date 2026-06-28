'use client'

import {
  createContext,
  useCallback,
  useContext,
  useMemo,
  useSyncExternalStore,
  type ReactNode,
} from 'react'
import { useRouter } from 'next/navigation'

import { apiPost } from '@/lib/api/client'
import {
  clearSession,
  getClientSessionSnapshot,
  getServerSessionSnapshot,
  subscribeToSession,
} from '@/lib/auth/session'
import { getAuthTokenFromCookie } from '@/lib/auth/cookie'
import type { AuthUser, RoleName } from '@/types/auth'

interface AuthContextValue {
  user: AuthUser | null
  token: string | null
  isLoading: boolean
  isAuthenticated: boolean
  login: (email: string) => Promise<void>
  logout: () => Promise<void>
  hasRole: (role: RoleName) => boolean
  hasAnyRole: (roles: RoleName[]) => boolean
}

const AuthContext = createContext<AuthContextValue | null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
  const router = useRouter()
  const session = useSyncExternalStore(
    subscribeToSession,
    getClientSessionSnapshot,
    getServerSessionSnapshot,
  )
  const user = session.user
  const token = session.token
  const isLoading = session.isLoading

  const logout = useCallback(async () => {
    const currentToken = getAuthTokenFromCookie()

    try {
      if (currentToken) {
        await apiPost<void>('api/auth/logout', undefined, currentToken)
      }
    } catch {
      // Limpa a sessão local mesmo se a Api falhar.
    }

    await clearSession()
    router.push('/auth')
  }, [router])

  const hasRole = useCallback(
    (role: RoleName) => user?.roles.includes(role) ?? false,
    [user],
  )

  const hasAnyRole = useCallback(
    (roles: RoleName[]) => roles.some((role) => user?.roles.includes(role)),
    [user],
  )

  const login = useCallback(async (email: string): Promise<void> => {
    await apiPost<void>('api/auth/login', {
      email: email.trim(),
      appOrigin: window.location.origin,
    })
  }, [])

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      token,
      isLoading,
      isAuthenticated: Boolean(token && user),
      login,
      logout,
      hasRole,
      hasAnyRole,
    }),
    [user, token, isLoading, login, logout, hasRole, hasAnyRole],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth deve ser usado dentro de AuthProvider')
  }
  return context
}

export { establishSession } from '@/lib/auth/session'
