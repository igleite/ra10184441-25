'use client'

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import type { ReactNode } from 'react'

import { ProtectedAreaGate } from '@/components/auth/protected-area-gate'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { usePermissions } from '@/hooks/use-permissions'
import { isNavItemActive } from '@/lib/nav'
import { cn } from '@/lib/utils'
import { buildRootUrl } from '@/lib/tenant'
import { useTenant } from '@/providers/tenant-provider'
import { useAuth } from '@/providers/auth-provider'

import { ROLES } from '@/types/auth'

function AppLayoutInner({ children }: { children: ReactNode }) {
  const pathname = usePathname()
  const { organization, slug } = useTenant()
  const { hasAnyRole, hasRole, logout, user } = useAuth()
  const { appNavItems, canAccessAppRoute } = usePermissions()

  const isSuperAdmin = hasRole(ROLES.superAdmin)
  const hasAppRole = hasAnyRole([
    ROLES.superAdmin,
    ROLES.organizationOwner,
    ROLES.organizationMember,
  ])

  const isAllowed = isSuperAdmin || (hasAppRole && canAccessAppRoute(pathname))

  const pendingOwnerRedirect = buildRootUrl(
    '/account/organizations/new',
    typeof window !== 'undefined' ? window.location.protocol : 'http:',
  )

  return (
    <ProtectedAreaGate
      isAllowed={isAllowed}
      deniedRedirect={
        hasRole(ROLES.onboarding) ? pendingOwnerRedirect : '/403'
      }
    >
      <div className="flex min-h-svh flex-col lg:flex-row">
        <header className="border-b lg:hidden">
          <div className="flex items-center justify-between px-4 py-3">
            <div>
              <p className="text-xs text-muted-foreground">{organization?.name ?? slug}</p>
              <p className="text-sm font-medium">App</p>
            </div>
            <Button variant="outline" size="sm" onClick={() => void logout()}>
              Sair
            </Button>
          </div>
          <nav className="flex gap-1 overflow-x-auto px-4 pb-3">
            {appNavItems.map((item) => (
              <Link
                key={item.href}
                href={item.href}
                className={cn(
                  'rounded-md px-3 py-1.5 text-sm whitespace-nowrap hover:bg-muted',
                  isNavItemActive(pathname, item.href)
                    ? 'bg-muted font-medium'
                    : 'text-muted-foreground',
                )}
              >
                {item.label}
              </Link>
            ))}
          </nav>
        </header>
        <aside className="hidden w-64 shrink-0 border-r bg-muted/30 p-4 lg:block">
          <div className="mb-6">
            <p className="text-xs font-medium uppercase tracking-wide text-muted-foreground">
              {organization?.name ?? slug}
            </p>
            <p className="text-sm">{user?.userName}</p>
          </div>
          <nav className="space-y-1">
            {appNavItems.map((item) => (
              <Link
                key={item.href}
                href={item.href}
                className={cn(
                  'block rounded-md px-3 py-2 text-sm hover:bg-muted',
                  isNavItemActive(pathname, item.href)
                    ? 'bg-muted font-medium'
                    : 'text-muted-foreground',
                )}
              >
                {item.label}
              </Link>
            ))}
          </nav>
          <Separator className="my-4" />
          <Button variant="outline" size="sm" onClick={() => void logout()}>
            Sair
          </Button>
        </aside>
        <main className="flex-1 p-6">{children}</main>
      </div>
    </ProtectedAreaGate>
  )
}

export default function AppLayout({ children }: { children: ReactNode }) {
  return <AppLayoutInner>{children}</AppLayoutInner>
}
