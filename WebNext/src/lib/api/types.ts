export interface ApiError {
  statusCode: number
  message: string[]
}

export function formatApiErrorMessage(
  error: unknown,
  fallback = 'Erro desconhecido',
): string {
  if (error && typeof error === 'object' && 'message' in error) {
    const message = (error as { message?: unknown }).message

    if (Array.isArray(message)) {
      const text = message
        .filter((item): item is string => typeof item === 'string')
        .join(' ')
      if (text) {
        return text
      }
    }

    if (typeof message === 'string' && message.length > 0) {
      return message
    }
  }

  if (error instanceof Error && error.message) {
    return error.message
  }

  return fallback
}

export interface ApiResult<T> {
  data: T
  statusCode: number
  message: string[]
}

export interface PageDto<T> {
  pageIndex: number
  pageSize: number
  totalItemCount: number
  items: T[] | null
}
