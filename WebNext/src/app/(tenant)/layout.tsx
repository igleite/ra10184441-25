'use client'

import type { ReactNode } from 'react'

import { TenantProvider } from '@/providers/tenant-provider'

export default function TenantLayout({ children }: { children: ReactNode }) {
  return <TenantProvider>{children}</TenantProvider>
}
