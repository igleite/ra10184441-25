export const ROLES = {
  superAdmin: 'SuperAdmin',
  onboarding: 'Onboarding',
  organizationOwner: 'OrganizationOwner',
  organizationMember: 'OrganizationMember',
  clientAdmin: 'ClientAdmin',
  clientMember: 'ClientMember',
  nullable: 'Nullable',
} as const

export type RoleName = (typeof ROLES)[keyof typeof ROLES]

export interface ClaimDto {
  type: string
  value: string
}

export interface AuthUser {
  userId: string
  userName: string
  email: string
  roles: RoleName[]
  organizationId?: string
  customerId?: string
}

export interface LoginResponse {
  token: string
  expiresAt: string
  user: {
    id: string
    email: string
    userName: string
    claims: ClaimDto[]
  }
}

export interface SessionUserDto {
  id: string
  email: string
  userName: string
  claims: ClaimDto[]
}

export interface OrganizationDto {
  id: string
  name: string
  document: string
  slug: string
  createdAt?: string
  updatedAt?: string
  rowVersion?: string
}
