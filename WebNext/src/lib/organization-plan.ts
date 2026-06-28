export interface OrganizationPlanDto {
  id: string
  planId: string
  description: string
  maxUsers: number
  maxClients: number
  maxTickets: number
  rowVersion: string
  inactivedAt?: string | null
}

export const ORGANIZATION_PLAN_ASSOCIATE_BLOCKED_MESSAGE =
  'Já existe um plano ativo nesta organização. Inative o plano atual antes de associar outro.'

export const ORGANIZATION_PLAN_ASSOCIATE_BLOCKED_TOOLTIP =
  'Inative o plano ativo atual para associar um novo vínculo.'

export function isOrganizationPlanActive(
  plan: Pick<OrganizationPlanDto, 'inactivedAt'>,
): boolean {
  return plan.inactivedAt == null || plan.inactivedAt === ''
}

export function getOrganizationPlanStatusLabel(
  plan: Pick<OrganizationPlanDto, 'inactivedAt'>,
): 'Ativo' | 'Inativo' {
  return isOrganizationPlanActive(plan) ? 'Ativo' : 'Inativo'
}

export function findActiveOrganizationPlan(
  plans: OrganizationPlanDto[],
): OrganizationPlanDto | undefined {
  return plans.find(isOrganizationPlanActive)
}

export function canAssociateOrganizationPlan(plans: OrganizationPlanDto[]): boolean {
  return !findActiveOrganizationPlan(plans)
}
