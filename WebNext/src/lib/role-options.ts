export const ROLE_IDS = {
  organizationOwner: 'e597e22c-e22e-4849-928c-e919717d2b8c',
  organizationMember: 'dc8c024b-69e2-4800-af50-f0fbd690279d',
  clientAdmin: '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5',
  clientMember: '11fe3aaa-d36d-4cae-9bce-7c5084360425',
} as const

export const TEAM_ROLE_OPTIONS = [
  { value: ROLE_IDS.organizationOwner, label: 'Dono da organização' },
  { value: ROLE_IDS.organizationMember, label: 'Membro da organização' },
] as const

export const CLIENT_ROLE_OPTIONS = [
  { value: ROLE_IDS.clientAdmin, label: 'Administrador do cliente' },
  { value: ROLE_IDS.clientMember, label: 'Membro do cliente' },
] as const

export function roleLabel(roleId: string): string {
  const match = [...TEAM_ROLE_OPTIONS, ...CLIENT_ROLE_OPTIONS].find(
    (option) => option.value === roleId,
  )
  return match?.label ?? roleId
}
