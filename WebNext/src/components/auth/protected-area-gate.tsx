'use client'

import type { ReactNode } from 'react'

import { Loading } from '@/components/shared/loading'
import { useProtectedLayout } from '@/hooks/use-protected-layout'

interface ProtectedAreaGateProps {
  children: ReactNode
  isAllowed: boolean
  authRedirect?: string
  deniedRedirect?: string
}

export function ProtectedAreaGate({
  children,
  isAllowed,
  authRedirect,
  deniedRedirect,
}: ProtectedAreaGateProps) {
  const { isBootstrapping, isCheckingAccess, isReady } = useProtectedLayout({
    isAllowed,
    authRedirect,
    deniedRedirect,
  })

  if (isBootstrapping) {
    return <Loading fullPage label="Restaurando sessão..." />
  }

  if (isCheckingAccess || !isReady) {
    return <Loading fullPage label="Verificando permissões..." />
  }

  return children
}
