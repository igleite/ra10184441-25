'use client'

import { Suspense, useEffect } from 'react'
import { useRouter } from 'next/navigation'

import { PublicAuthLoginPage } from '@/components/auth/public-auth-login-page'
import { Loading } from '@/components/shared/loading'
import { isRootDomain } from '@/lib/tenant'
import { TenantProvider } from '@/providers/tenant-provider'
import { useAuth } from '@/providers/auth-provider'

import TenantAuthLoginPage from '@/app/(tenant)/auth/tenant-auth-login-page'

function AuthPageContent() {
  const { isAuthenticated, isLoading } = useAuth()
  const router = useRouter()
  const host = typeof window !== 'undefined' ? window.location.hostname : ''

  useEffect(() => {
    if (isLoading) return
    if (!isAuthenticated) return
    if (isRootDomain(host)) {
      router.replace('/account')
    } else {
      router.replace('/app')
    }
  }, [isAuthenticated, isLoading, host, router])

  if (isLoading || isAuthenticated) return null

  if (isRootDomain(host)) {
    return <PublicAuthLoginPage />
  }

  return (
    <Suspense fallback={<Loading fullPage label="Carregando..." />}>
      <TenantProvider>
        <TenantAuthLoginPage />
      </TenantProvider>
    </Suspense>
  )
}

export default function AuthPage() {
  return <AuthPageContent />
}
