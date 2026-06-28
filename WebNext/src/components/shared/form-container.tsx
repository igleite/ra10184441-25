'use client'

import Link from 'next/link'
import type { FormEvent, ReactNode } from 'react'

import { FormFeedback } from '@/components/shared/form-feedback'
import { Button } from '@/components/ui/button'

export interface FormContainerProps {
  onSubmit: (event: FormEvent<HTMLFormElement>) => void | Promise<void>
  isSubmitting?: boolean
  children: ReactNode
  submitLabel?: string
  cancelHref?: string
  error?: string | null
  success?: string | null
  hideSubmit?: boolean
}

export function FormContainer({
  onSubmit,
  isSubmitting = false,
  children,
  submitLabel = 'Salvar',
  cancelHref,
  error,
  success,
  hideSubmit = false,
}: FormContainerProps) {
  return (
    <form
      className="space-y-6"
      onSubmit={(event) => {
        event.preventDefault()
        void onSubmit(event)
      }}
    >
      <FormFeedback error={error} success={success} />
      {children}
      {hideSubmit ? null : (
        <div className="flex items-center gap-2">
          <Button type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Salvando...' : submitLabel}
          </Button>
          {cancelHref ? (
            <Button variant="outline" asChild>
              <Link href={cancelHref}>Cancelar</Link>
            </Button>
          ) : null}
        </div>
      )}
    </form>
  )
}
