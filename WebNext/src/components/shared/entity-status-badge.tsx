import { cn } from '@/lib/utils'

export interface EntityStatusBadgeProps {
  active: boolean
  activeLabel?: string
  inactiveLabel?: string
}

export function EntityStatusBadge({
  active,
  activeLabel = 'Ativo',
  inactiveLabel = 'Inativo',
}: EntityStatusBadgeProps) {
  return (
    <span
      className={cn(
        'inline-flex rounded-full px-2 py-0.5 text-xs font-medium',
        active
          ? 'bg-emerald-500/10 text-emerald-700 dark:text-emerald-400'
          : 'bg-muted text-muted-foreground',
      )}
    >
      {active ? activeLabel : inactiveLabel}
    </span>
  )
}
