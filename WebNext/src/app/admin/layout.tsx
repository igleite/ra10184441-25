'use client'

import Link from 'next/link'
import { usePathname, useRouter } from 'next/navigation'
import { useEffect, type ReactNode } from 'react'

import { ProtectedAreaGate } from '@/components/auth/protected-area-gate'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { usePermissions } from '@/hooks/use-permissions'
import { isNavItemActive } from '@/lib/nav'
import { cn } from '@/lib/utils'
import { useAuth } from '@/providers/auth-provider'
import { ROLES } from '@/types/auth'

export default function AdminLayout({ children }: { children: ReactNode }) {
  const pathname = usePathname()
  const router = useRouter()
  const { hasRole, isLoading, logout, user } = useAuth()
  const { adminNavItems, canAccessAdminRoute } = usePermissions()

  const isSuperAdmin = hasRole(ROLES.superAdmin)
  const hasAdminAreaAccess = isSuperAdmin || adminNavItems.length > 0
  const canAccessCurrentRoute = isSuperAdmin || canAccessAdminRoute(pathname)

  useEffect(() => {
    if (isLoading || !hasAdminAreaAccess || canAccessCurrentRoute) {
      return
    }

    const fallbackHref = adminNavItems[0]?.href
    if (fallbackHref && fallbackHref !== pathname) {
      router.replace(fallbackHref)
    }
  }, [
    adminNavItems,
    canAccessCurrentRoute,
    hasAdminAreaAccess,
    isLoading,
    pathname,
    router,
  ])

  return (
    <ProtectedAreaGate
      isAllowed={isSuperAdmin}
      deniedRedirect={
        hasRole(ROLES.onboarding) ? '/account/organizations/new' : '/403'
      }
    >
      <div className="flex min-h-svh">
        <aside className="hidden w-64 shrink-0 border-r bg-muted/30 p-4 md:block">
          <div className="mb-6">
            <p className="text-xs font-medium uppercase tracking-wide text-muted-foreground">
              Administração
            </p>
            <p className="text-sm">{user?.userName}</p>
          </div>
          <nav className="space-y-1">
            {adminNavItems.map((item) => (
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
        <div className="flex min-w-0 flex-1 flex-col">
          <header className="border-b md:hidden">
            <div className="flex items-center justify-between px-4 py-3">
              <span className="font-medium">Admin</span>
              <Button variant="outline" size="sm" onClick={() => void logout()}>
                Sair
              </Button>
            </div>
            <nav className="flex gap-1 overflow-x-auto px-4 pb-3">
              {adminNavItems.map((item) => (
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
          <main className="flex-1 p-6">
            {!isLoading && hasAdminAreaAccess && !canAccessCurrentRoute ? null : children}
          </main>
        </div>
      </div>
    </ProtectedAreaGate>
  )
}
