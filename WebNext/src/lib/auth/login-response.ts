import type { ClaimDto, LoginResponse, SessionUserDto } from '@/types/auth'

function normalizeClaims(raw: unknown): ClaimDto[] {
  if (!Array.isArray(raw)) {
    return []
  }

  return raw
    .map((claim) => {
      if (!claim || typeof claim !== 'object') {
        return null
      }

      const entry = claim as Record<string, unknown>
      const type = String(entry.type ?? entry.Type ?? '')
      const value = String(entry.value ?? entry.Value ?? '')

      if (!type || !value) {
        return null
      }

      return { type, value }
    })
    .filter((claim): claim is ClaimDto => claim !== null)
}

export function normalizeSessionUserDto(data: SessionUserDto): SessionUserDto {
  const raw = data as SessionUserDto & {
    Id?: string
    Email?: string
    UserName?: string
    Claims?: ClaimDto[]
  }

  return {
    id: data.id ?? raw.Id ?? '',
    email: data.email ?? raw.Email ?? '',
    userName: data.userName ?? raw.UserName ?? '',
    claims: normalizeClaims(data.claims ?? raw.Claims),
  }
}

export function normalizeLoginResponse(data: LoginResponse): LoginResponse {
  const raw = data as LoginResponse & {
    Token?: string
    ExpiresAt?: string
    User?: {
      Id?: string
      Email?: string
      UserName?: string
      Claims?: ClaimDto[]
    }
  }

  const user = data.user ?? raw.User

  return {
    token: (data.token ?? raw.Token ?? '').trim(),
    expiresAt: data.expiresAt ?? raw.ExpiresAt ?? '',
    user: {
      id: user?.id ?? raw.User?.Id ?? '',
      email: user?.email ?? raw.User?.Email ?? '',
      userName: user?.userName ?? raw.User?.UserName ?? '',
      claims: normalizeClaims(user?.claims ?? raw.User?.Claims),
    },
  }
}
