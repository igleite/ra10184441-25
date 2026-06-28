import { cn } from '@/lib/utils'

export interface FormFeedbackProps {
  error?: string | null
  success?: string | null
  className?: string
}

export function FormFeedback({ error, success, className }: FormFeedbackProps) {
  if (!error && !success) {
    return null
  }

  return (
    <div className={cn('space-y-2', className)}>
      {error ? (
        <p className="rounded-md border border-destructive/30 bg-destructive/5 px-3 py-2 text-sm text-destructive" role="alert">
          {error}
        </p>
      ) : null}
      {success ? (
        <p className="rounded-md border border-emerald-500/30 bg-emerald-500/5 px-3 py-2 text-sm text-emerald-700 dark:text-emerald-400" role="status">
          {success}
        </p>
      ) : null}
    </div>
  )
}
