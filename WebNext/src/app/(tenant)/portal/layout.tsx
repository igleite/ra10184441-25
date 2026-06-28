'use client'

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import type { ReactNode } from 'react'

import { ProtectedAreaGate } from '@/components/auth/protected-area-gate'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { CustomerProvider } from '@/providers/customer-provider'
import { useTenant } from '@/providers/tenant-provider'
import { useAuth } from '@/providers/auth-provider'
import { usePermissions } from '@/hooks/use-permissions'
import { ROLES } from '@/types/auth'
import { isNavItemActive } from '@/lib/nav'
import { cn } from '@/lib/utils'
import { buildRootUrl } from '@/lib/tenant'

function PortalLayoutInner({ children }: { children: ReactNode }) {
  const pathname = usePathname()
  const { organization, slug } = useTenant()
  const { hasAnyRole, hasRole, logout } = useAuth()
  const { portalNavItems, canAccessPortalRoute } = usePermissions()

  const isSuperAdmin = hasRole(ROLES.superAdmin)
  const hasPortalRole = hasAnyRole([ROLES.clientAdmin, ROLES.clientMember])
  const isAllowed =
    isSuperAdmin || (hasPortalRole && canAccessPortalRoute(pathname))

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
      <CustomerProvider>
        <div className="flex min-h-svh flex-col">
          <header className="border-b bg-background">
            <div className="mx-auto flex max-w-6xl items-center justify-between px-4 py-3">
              <div>
                <p className="text-xs text-muted-foreground">Portal do cliente</p>
                <p className="font-medium">{organization?.name ?? slug}</p>
              </div>
              <Button variant="outline" size="sm" onClick={() => void logout()}>
                Sair
              </Button>
            </div>
            <nav className="mx-auto flex max-w-6xl gap-1 overflow-x-auto px-4 pb-3">
              {portalNavItems.map((item) => (
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
            <Separator />
          </header>
          <main className="mx-auto w-full max-w-6xl flex-1 p-6">{children}</main>
        </div>
      </CustomerProvider>
    </ProtectedAreaGate>
  )
}

export default function PortalLayout({ children }: { children: ReactNode }) {
  return <PortalLayoutInner>{children}</PortalLayoutInner>
}
