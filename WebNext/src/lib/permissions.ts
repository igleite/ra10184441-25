import { ROLES, type RoleName } from '@/types/auth'

export type PermissionAction =
  | 'admin:access'
  | 'admin:organizations'
  | 'admin:users'
  | 'admin:feature-flags'
  | 'account:access'
  | 'app:access'
  | 'portal:access'
  | 'tickets:read'
  | 'tickets:create'
  | 'tickets:edit'
  | 'customers:read'
  | 'customers:manage'
  | 'plans:read'
  | 'plans:link'
  | 'plans:edit-description'
  | 'plans:edit-limits'
  | 'plans:delete'
  | 'users-org:manage'
  | 'teams:manage'
  | 'catalog:manage'
  | 'settings:edit'
  | 'portal-tickets:read'
  | 'portal-tickets:create'
  | 'portal-artifacts:read'
  | 'portal-artifacts:manage'
  | 'portal-users:manage'

const ALL_APP_TEAM: RoleName[] = [
  ROLES.superAdmin,
  ROLES.organizationOwner,
  ROLES.organizationMember,
]

const APP_MANAGEMENT: RoleName[] = [ROLES.superAdmin, ROLES.organizationOwner]

const PORTAL_ALL: RoleName[] = [ROLES.clientAdmin, ROLES.clientMember]

const ACTION_ROLES: Record<PermissionAction, RoleName[]> = {
  'admin:access': [ROLES.superAdmin],
  'admin:organizations': [ROLES.superAdmin],
  'admin:users': [ROLES.superAdmin],
  'admin:feature-flags': [ROLES.superAdmin],
  'account:access': [ROLES.organizationOwner],
  'app:access': ALL_APP_TEAM,
  'portal:access': PORTAL_ALL,

  'tickets:read': ALL_APP_TEAM,
  'tickets:create': ALL_APP_TEAM,
  'tickets:edit': ALL_APP_TEAM,

  'customers:read': ALL_APP_TEAM,
  'customers:manage': APP_MANAGEMENT,

  'plans:read': APP_MANAGEMENT,
  'plans:link': APP_MANAGEMENT,
  'plans:edit-description': [ROLES.superAdmin],
  'plans:edit-limits': [ROLES.superAdmin],
  'plans:delete': APP_MANAGEMENT,

  'users-org:manage': APP_MANAGEMENT,
  'teams:manage': APP_MANAGEMENT,
  'catalog:manage': APP_MANAGEMENT,
  'settings:edit': APP_MANAGEMENT,

  'portal-tickets:read': PORTAL_ALL,
  'portal-tickets:create': PORTAL_ALL,
  'portal-artifacts:read': PORTAL_ALL,
  'portal-artifacts:manage': [ROLES.clientAdmin],
  'portal-users:manage': [ROLES.clientAdmin],
}

export interface AppNavItem {
  href: string
  label: string
  exact?: boolean
  action?: PermissionAction
}

export interface PortalNavItem {
  href: string
  label: string
  action?: PermissionAction
}

export interface AdminNavItem {
  href: string
  label: string
  exact?: boolean
  action?: PermissionAction
}

export const ADMIN_NAV_ITEMS: AdminNavItem[] = [
  { href: '/admin', label: 'Dashboard', exact: true, action: 'admin:access' },
  { href: '/admin/organizations', label: 'Organizações', action: 'admin:organizations' },
  { href: '/admin/users', label: 'Usuários', action: 'admin:users' },
  { href: '/admin/feature-flags', label: 'Feature flags', action: 'admin:feature-flags' },
]

export const APP_NAV_ITEMS: AppNavItem[] = [
  { href: '/app', label: 'Dashboard', exact: true, action: 'app:access' },
  { href: '/app/tickets', label: 'Tickets', action: 'tickets:read' },
  { href: '/app/customers', label: 'Clientes', action: 'customers:read' },
  // { href: '/app/plans', label: 'Planos', action: 'plans:read' },
  { href: '/app/users', label: 'Usuários', action: 'users-org:manage' },
  { href: '/app/teams', label: 'Times', action: 'teams:manage' },
  { href: '/app/ticket-classifications', label: 'Classificações', action: 'catalog:manage' },
  { href: '/app/ticket-status-reasons', label: 'Motivos de status', action: 'catalog:manage' },
  { href: '/app/artifact-types', label: 'Tipos de artefato', action: 'catalog:manage' },
  { href: '/app/settings', label: 'Configurações', action: 'settings:edit' },
  { href: '/app/profile', label: 'Perfil', action: 'app:access' },
]

export const PORTAL_NAV_ITEMS: PortalNavItem[] = [
  { href: '/portal', label: 'Dashboard', action: 'portal:access' },
  { href: '/portal/tickets', label: 'Tickets', action: 'portal-tickets:read' },
  { href: '/portal/artifacts', label: 'Artefatos', action: 'portal-artifacts:read' },
  { href: '/portal/users', label: 'Usuários', action: 'portal-users:manage' },
  { href: '/portal/profile', label: 'Perfil', action: 'portal:access' },
]

const APP_MANAGEMENT_PREFIXES = [
  '/app/settings',
  '/app/users',
  '/app/teams',
  '/app/plans',
  '/app/ticket-classifications',
  '/app/ticket-status-reasons',
  '/app/artifact-types',
] as const

const APP_CUSTOMER_ADMIN_PATTERN = /^\/app\/customers\/[^/]+\/(users|artifacts)(\/|$)/

const PORTAL_USERS_PATTERN = /^\/portal\/users(\/|$)/

const ADMIN_ORGANIZATIONS_PATTERN = /^\/admin\/organizations(\/|$)/
const ADMIN_USERS_PATTERN = /^\/admin\/users(\/|$)/
const ADMIN_FEATURE_FLAGS_PATTERN = /^\/admin\/feature-flags(\/|$)/

export function hasPermission(roles: RoleName[], action: PermissionAction): boolean {
  const allowed = ACTION_ROLES[action]
  return roles.some((role) => allowed.includes(role))
}

export function canAccessAppRoute(pathname: string, roles: RoleName[]): boolean {
  if (!hasPermission(roles, 'app:access')) {
    return false
  }

  if (APP_MANAGEMENT_PREFIXES.some((prefix) => pathname.startsWith(prefix))) {
    return roles.some((role) => APP_MANAGEMENT.includes(role))
  }

  if (APP_CUSTOMER_ADMIN_PATTERN.test(pathname)) {
    return hasPermission(roles, 'customers:manage')
  }

  return true
}

export function canAccessPortalRoute(pathname: string, roles: RoleName[]): boolean {
  if (!hasPermission(roles, 'portal:access')) {
    return false
  }

  if (PORTAL_USERS_PATTERN.test(pathname)) {
    return hasPermission(roles, 'portal-users:manage')
  }

  return true
}

export function canAccessAdminRoute(pathname: string, roles: RoleName[]): boolean {
  if (filterAdminNavItems(roles).length === 0) {
    return false
  }

  if (pathname === '/admin' || pathname === '/admin/dashboard') {
    return hasPermission(roles, 'admin:access')
  }

  if (ADMIN_ORGANIZATIONS_PATTERN.test(pathname)) {
    return hasPermission(roles, 'admin:organizations')
  }

  if (ADMIN_USERS_PATTERN.test(pathname)) {
    return hasPermission(roles, 'admin:users')
  }

  if (ADMIN_FEATURE_FLAGS_PATTERN.test(pathname)) {
    return hasPermission(roles, 'admin:feature-flags')
  }

  return hasPermission(roles, 'admin:access')
}

export function filterAppNavItems(roles: RoleName[]): AppNavItem[] {
  return APP_NAV_ITEMS.filter((item) => {
    const action = item.action ?? 'app:access'
    return hasPermission(roles, action)
  })
}

export function filterPortalNavItems(roles: RoleName[]): PortalNavItem[] {
  return PORTAL_NAV_ITEMS.filter((item) => {
    const action = item.action ?? 'portal:access'
    return hasPermission(roles, action)
  })
}

export function filterAdminNavItems(roles: RoleName[]): AdminNavItem[] {
  return ADMIN_NAV_ITEMS.filter((item) => {
    const action = item.action ?? 'admin:access'
    return hasPermission(roles, action)
  })
}

export function getPrimaryAppRole(roles: RoleName[]): RoleName | null {
  if (roles.includes(ROLES.superAdmin)) {
    return ROLES.superAdmin
  }
  if (roles.includes(ROLES.organizationOwner)) {
    return ROLES.organizationOwner
  }
  if (roles.includes(ROLES.organizationMember)) {
    return ROLES.organizationMember
  }
  return null
}

export function getPrimaryPortalRole(roles: RoleName[]): RoleName | null {
  if (roles.includes(ROLES.clientAdmin)) {
    return ROLES.clientAdmin
  }
  if (roles.includes(ROLES.clientMember)) {
    return ROLES.clientMember
  }
  return null
}
