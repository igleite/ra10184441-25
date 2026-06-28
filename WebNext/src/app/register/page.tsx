'use client'

import Link from 'next/link'
import { useState } from 'react'

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
import { apiPost } from '@/lib/api/client'
import { formatApiErrorMessage } from '@/lib/api/types'

export default function PublicRegisterPage() {
  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [registered, setRegistered] = useState(false)

  async function handleSubmit() {
    setError(null)
    setIsSubmitting(true)

    try {
      await apiPost<void>('api/auth/register', {
        name: name.trim(),
        email: email.trim(),
      })
      setRegistered(true)
    } catch (err) {
      setError(formatApiErrorMessage(err, 'Não foi possível criar a conta.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  if (registered) {
    return (
      <div className="flex min-h-svh items-center justify-center px-4">
        <Card className="w-full max-w-md">
          <CardHeader>
            <CardTitle>Conta criada</CardTitle>
            <CardDescription>
              Enviamos um link de acesso para{' '}
              <span className="font-medium">{email.trim()}</span>.
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <p className="text-sm text-muted-foreground">
              Clique no link recebido para entrar na plataforma. O link expira em
              alguns minutos.
            </p>
            <Button variant="outline" className="w-full" asChild>
              <Link href="/auth">Ir para o login</Link>
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
          <CardTitle>Criar conta</CardTitle>
          <CardDescription>
            Cadastre sua conta global na plataforma. Após o cadastro, você receberá
            um link de acesso por e-mail.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <FormContainer
            onSubmit={handleSubmit}
            isSubmitting={isSubmitting}
            submitLabel="Criar conta"
          >
            <div className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="name">Nome</Label>
                <Input
                  id="name"
                  type="text"
                  autoComplete="name"
                  value={name}
                  onChange={(event) => setName(event.target.value)}
                  required
                />
              </div>
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
                Não usamos senha — o acesso é feito por magic link.
              </p>
            </div>
          </FormContainer>
          <Button variant="link" className="mt-4 h-auto p-0" asChild>
            <Link href="/auth">Já tenho conta</Link>
          </Button>
        </CardContent>
      </Card>
    </div>
  )
}
