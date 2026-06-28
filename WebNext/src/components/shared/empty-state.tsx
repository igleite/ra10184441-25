import Link from 'next/link'
import type { ReactNode } from 'react'

import { Button } from '@/components/ui/button'

export interface EmptyStateAction {
  label: string
  href?: string
  onClick?: () => void
}

export interface EmptyStateProps {
  title: string
  description?: string
  action?: EmptyStateAction
}

export function EmptyState({ title, description, action }: EmptyStateProps) {
  let actionNode: ReactNode = null

  if (action) {
    if (action.href) {
      actionNode = (
        <Button asChild>
          <Link href={action.href}>{action.label}</Link>
        </Button>
      )
    } else if (action.onClick) {
      actionNode = <Button onClick={action.onClick}>{action.label}</Button>
    }
  }

  return (
    <div className="flex flex-col items-center justify-center rounded-lg border border-dashed px-6 py-16 text-center">
      <h3 className="text-base font-medium">{title}</h3>
      {description ? (
        <p className="mt-2 max-w-md text-sm text-muted-foreground">{description}</p>
      ) : null}
      {actionNode ? <div className="mt-4">{actionNode}</div> : null}
    </div>
  )
}
