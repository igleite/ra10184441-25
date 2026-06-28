import type {
  AuthUser,
  ClaimDto,
  LoginResponse,
  RoleName,
  SessionUserDto,
} from '@/types/auth'

function getClaimType(claim: ClaimDto): string {
  return claim.type ?? (claim as { Type?: string }).Type ?? ''
}

function getClaimValue(claim: ClaimDto): string {
  return claim.value ?? (claim as { Value?: string }).Value ?? ''
}

function parseRolesFromJwt(token: string): RoleName[] {
  try {
    const payload = token.split('.')[1]
    if (!payload) {
      return []
    }

    const decoded = atob(payload.replace(/-/g, '+').replace(/_/g, '/'))
    const parsed = JSON.parse(decoded) as Record<string, unknown>
    const role = parsed.role ?? parsed.Role

    if (Array.isArray(role)) {
      return role.filter((value): value is RoleName => typeof value === 'string')
    }

    if (typeof role === 'string' && role.length > 0) {
      return [role as RoleName]
    }
  } catch {
    // ignore decode errors
  }

  return []
}

function parseRoles(claims: ClaimDto[], token?: string): RoleName[] {
  const fromClaims = claims
    .filter((claim) => getClaimType(claim) === 'role')
    .map((claim) => getClaimValue(claim) as RoleName)
    .filter(Boolean)

  if (fromClaims.length > 0) {
    return fromClaims
  }

  if (token) {
    return parseRolesFromJwt(token)
  }

  return []
}

function decodeJwtPayload(token: string): Record<string, string> {
  try {
    const payload = token.split('.')[1]
    if (!payload) {
      return {}
    }

    const decoded = atob(payload.replace(/-/g, '+').replace(/_/g, '/'))
    const parsed = JSON.parse(decoded) as Record<string, unknown>

    return Object.fromEntries(
      Object.entries(parsed).map(([key, value]) => [key, String(value)]),
    )
  } catch {
    return {}
  }
}

function withJwtContext(user: AuthUser, token: string): AuthUser {
  const jwtClaims = decodeJwtPayload(token)

  return {
    ...user,
    organizationId: jwtClaims.organization_id,
    customerId: jwtClaims.customer_id,
  }
}

export function buildAuthUserFromLogin(login: LoginResponse): AuthUser {
  return withJwtContext(
    {
      userId: login.user.id,
      userName: login.user.userName,
      email: login.user.email,
      roles: parseRoles(login.user.claims, login.token),
    },
    login.token,
  )
}

export function buildAuthUserFromSessionUser(
  user: SessionUserDto,
  token: string,
): AuthUser {
  return withJwtContext(
    {
      userId: user.id,
      userName: user.userName,
      email: user.email,
      roles: parseRoles(user.claims, token),
    },
    token,
  )
}
