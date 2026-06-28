'use client'

import Link from 'next/link'
import { useState, type ReactNode } from 'react'

import { FormContainer } from '@/components/shared/form-container'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { formatApiErrorMessage } from '@/lib/api/types'
import { useAuth } from '@/providers/auth-provider'

interface MagicLinkLoginFormProps {
  title: string
  description: ReactNode
  successDescription?: ReactNode
  registerHref?: string
  onBeforeLogin?: () => void
}

export function MagicLinkLoginForm({
  title,
  description,
  successDescription,
  registerHref = '/register',
  onBeforeLogin,
}: MagicLinkLoginFormProps) {
  const { login } = useAuth()
  const [email, setEmail] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [linkSent, setLinkSent] = useState(false)

  async function handleSubmit() {
    setError(null)
    setIsSubmitting(true)

    try {
      onBeforeLogin?.()
      await login(email)
      setLinkSent(true)
    } catch (err) {
      setError(formatApiErrorMessage(err, 'Não foi possível enviar o link.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  if (linkSent) {
    return (
      <div className="flex min-h-svh items-center justify-center px-4">
        <Card className="w-full max-w-md">
          <CardHeader>
            <CardTitle>Verifique seu e-mail</CardTitle>
            <CardDescription>
              Enviamos um link de acesso para{' '}
              <span className="font-medium">{email.trim()}</span>.
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <p className="text-sm text-muted-foreground">
              {successDescription ??
                'Clique no link recebido para concluir o login. O link expira em alguns minutos.'}
            </p>
            <Button
              type="button"
              variant="outline"
              className="w-full"
              onClick={() => {
                setLinkSent(false)
                setError(null)
              }}
            >
              Usar outro e-mail
            </Button>
          </CardContent>
        </Card>
      </div>
    )
  }

  return (
    <div className="flex min-h-svh items-center justify-center px-4">
      <Card className="w-full max-w-md">
        <CardHeader>
          <CardTitle>{title}</CardTitle>
          <CardDescription>{description}</CardDescription>
        </CardHeader>
        <CardContent>
          <FormContainer
            onSubmit={handleSubmit}
            isSubmitting={isSubmitting}
            submitLabel="Enviar link de acesso"
          >
            <div className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="email">E-mail</Label>
                <Input
                  id="email"
                  type="email"
                  autoComplete="email"
                  value={email}
                  onChange={(event) => setEmail(event.target.value)}
                  required
                />
              </div>
              {error ? (
                <p className="text-sm text-destructive" role="alert">
                  {error}
                </p>
              ) : null}
              <p className="text-sm text-muted-foreground">
                Enviaremos um link seguro para o seu e-mail — sem senha.
              </p>
            </div>
          </FormContainer>
          {registerHref ? (
            <Button variant="link" className="mt-4 h-auto p-0" asChild>
              <Link href={registerHref}>Criar conta</Link>
            </Button>
          ) : null}
        </CardContent>
      </Card>
    </div>
  )
}
