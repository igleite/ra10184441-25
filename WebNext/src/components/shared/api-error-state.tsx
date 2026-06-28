'use client'

import { Button } from '@/components/ui/button'

export interface ApiErrorStateProps {
  title?: string
  message: string
  onRetry?: () => void
  retryLabel?: string
  backHref?: string
  backLabel?: string
}

export function ApiErrorState({
  title = 'Erro ao carregar',
  message,
  onRetry,
  retryLabel = 'Tentar novamente',
  backHref,
  backLabel = 'Voltar',
}: ApiErrorStateProps) {
  return (
    <div
      className="rounded-lg border border-destructive/30 bg-destructive/5 p-6 text-center"
      role="alert"
    >
      <h2 className="text-base font-semibold text-destructive">{title}</h2>
      <p className="mt-2 text-sm text-muted-foreground">{message}</p>
      <div className="mt-4 flex flex-wrap justify-center gap-2">
        {onRetry ? (
          <Button type="button" variant="outline" onClick={onRetry}>
            {retryLabel}
          </Button>
        ) : null}
        {backHref ? (
          <Button type="button" variant="outline" asChild>
            <a href={backHref}>{backLabel}</a>
          </Button>
        ) : null}
      </div>
    </div>
  )
}
