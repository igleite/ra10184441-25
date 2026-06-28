import { apiGet } from '@/lib/api/client'
import { normalizeLoginResponse } from '@/lib/auth/login-response'
import type { LoginResponse } from '@/types/auth'

const verifyRequests = new Map<string, Promise<LoginResponse>>()
const completedVerifications = new Set<string>()

export function hasCompletedVerification(token: string): boolean {
  return completedVerifications.has(token)
}

export function markVerificationCompleted(token: string): void {
  completedVerifications.add(token)
}

export function verifyMagicLink(token: string): Promise<LoginResponse> {
  const cached = verifyRequests.get(token)
  if (cached) {
    return cached
  }

  const request = apiGet<LoginResponse>(
    `api/auth/verify?token=${encodeURIComponent(token)}`,
    null,
  ).then((login) => normalizeLoginResponse(login))

  verifyRequests.set(token, request)

  void request.finally(() => {
    verifyRequests.delete(token)
  })

  return request
}
