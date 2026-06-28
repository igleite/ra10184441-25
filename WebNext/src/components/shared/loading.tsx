import { Loader2Icon } from 'lucide-react'

import { cn } from '@/lib/utils'

export interface LoadingProps {
  fullPage?: boolean
  label?: string
}

export function Loading({ fullPage = false, label = 'Carregando...' }: LoadingProps) {
  return (
    <div
      className={cn(
        'flex items-center justify-center gap-2 text-sm text-muted-foreground',
        fullPage ? 'min-h-svh' : 'py-12',
      )}
    >
      <Loader2Icon className="size-4 animate-spin" />
      <span>{label}</span>
    </div>
  )
}
