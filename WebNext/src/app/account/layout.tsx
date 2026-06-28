'use client'

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import type { ReactNode } from 'react'

import { ProtectedAreaGate } from '@/components/auth/protected-area-gate'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { useAuth } from '@/providers/auth-provider'
import { ROLES } from '@/types/auth'
import { isNavItemActive } from '@/lib/nav'
import { cn } from '@/lib/utils'

const navItems = [
  { href: '/account', label: 'Dashboard' },
  { href: '/account/organizations', label: 'Organizações' },
  { href: '/account/profile', label: 'Perfil' },
]

function isOnboardingRoute(pathname: string): boolean {
  return (
    pathname === '/account' ||
    pathname === '/account/profile' ||
    pathname === '/account/organizations' ||
    pathname === '/account/organizations/new' ||
    pathname.startsWith('/account/organizations/new')
  )
}

export default function AccountLayout({ children }: { children: ReactNode }) {

  console.log('AccountLayout')

  const pathname = usePathname()
  const { hasRole, logout, user } = useAuth()
  
  const isOnboarding = hasRole(ROLES.onboarding)
  const isSuperAdmin = hasRole(ROLES.superAdmin)
  const hasAccountAccess = hasRole(ROLES.organizationOwner) || hasRole(ROLES.organizationMember) || hasRole(ROLES.onboarding) || hasRole(ROLES.nullable)
  
  const isAllowed = isSuperAdmin || hasAccountAccess || (isOnboarding && isOnboardingRoute(pathname))
  
    const deniedRedirect =
    isOnboarding && !isOnboardingRoute(pathname)
      ? '/account/organizations/new'
      : '/403'

  return (
    <ProtectedAreaGate
      isAllowed={isAllowed}
      deniedRedirect={deniedRedirect}
    >
      <div className="flex min-h-svh flex-col md:flex-row">
        <header className="border-b md:hidden">
          <div className="flex items-center justify-between px-4 py-3">
            <span className="font-medium">Minha conta</span>
            <Button variant="outline" size="sm" onClick={() => void logout()}>
              Sair
            </Button>
          </div>
          <nav className="flex gap-1 overflow-x-auto px-4 pb-3">
            {navItems.map((item) => (
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
        <aside className="hidden w-56 shrink-0 border-r bg-muted/20 p-4 md:block">
          <div className="mb-6">
            <p className="text-xs font-medium uppercase tracking-wide text-muted-foreground">
              Minha conta
            </p>
            <p className="text-sm">{user?.userName}</p>
          </div>
          <nav className="space-y-1">
            {navItems.map((item) => (
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
