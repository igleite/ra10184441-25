import { getAuthTokenFromCookie } from '@/lib/auth/cookie'
import type { ApiError, ApiResult } from '@/lib/api/types'

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? ''

function getAuthToken(): string | null {
  return getAuthTokenFromCookie()
}

function normalizeApiMessages(rawMessage: unknown): string[] {
  if (Array.isArray(rawMessage)) {
    const messages = rawMessage.filter((item): item is string => typeof item === 'string')
    return messages.length > 0 ? messages : ['Erro desconhecido']
  }

  if (typeof rawMessage === 'string' && rawMessage.length > 0) {
    return [rawMessage]
  }

  return ['Erro desconhecido']
}

async function parseResponse<T>(response: Response): Promise<T> {
  if (response.status === 204) {
    return undefined as T
  }

  const body = (await response.json()) as ApiResult<T> | ApiError & { Message?: unknown }

  if (!response.ok) {
    const error = body as ApiError & { Message?: unknown }
    const message = normalizeApiMessages(error.message ?? error.Message)

    throw {
      statusCode: error.statusCode ?? response.status,
      message,
    } satisfies ApiError
  }

  const result = body as ApiResult<T>
  return result.data
}

async function request<T>(
  method: string,
  path: string,
  options?: { body?: unknown; token?: string | null },
): Promise<T> {
  const token = options?.token === undefined ? getAuthToken() : options.token
  const headers: HeadersInit = {
    'Content-Type': 'application/json',
  }

  if (token) {
    headers.Authorization = `Bearer ${token}`
  }

  const response = await fetch(`${API_URL}/${path.replace(/^\//, '')}`, {
    method,
    headers,
    body: options?.body !== undefined ? JSON.stringify(options.body) : undefined,
  })

  return parseResponse<T>(response)
}

export function apiGet<T>(path: string, token?: string | null): Promise<T> {
  return request<T>('GET', path, { token })
}

export function apiPost<T>(
  path: string,
  body: unknown,
  token?: string | null,
): Promise<T> {
  return request<T>('POST', path, { body, token })
}

export function apiPut<T>(
  path: string,
  body: unknown,
  token?: string | null,
): Promise<T> {
  return request<T>('PUT', path, { body, token })
}

export function apiDelete<T>(
  path: string,
  token?: string | null,
): Promise<T> {
  return request<T>('DELETE', path, { token })
}

export { getAuthToken }
