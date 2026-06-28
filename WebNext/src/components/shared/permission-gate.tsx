'use client'

import type { ReactNode } from 'react'

import { usePermissions } from '@/hooks/use-permissions'
import type { PermissionAction } from '@/lib/permissions'

export interface PermissionGateProps {
  action: PermissionAction
  children: ReactNode
  fallback?: ReactNode
}

export function PermissionGate({
  action,
  children,
  fallback = null,
}: PermissionGateProps) {
  const { can } = usePermissions()

  if (!can(action)) {
    return fallback
  }

  return children
}
