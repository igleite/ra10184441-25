'use client'

import Link from 'next/link'
import { useSearchParams } from 'next/navigation'
import { Suspense, useEffect, useState } from 'react'

import { Loading } from '@/components/shared/loading'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { isAbsoluteUrl, resolvePostAuthRedirect } from '@/lib/auth/redirect'
import { establishSession, rebootstrapSession } from '@/lib/auth/session'
import {
  hasCompletedVerification,
  markVerificationCompleted,
  verifyMagicLink,
} from '@/lib/auth/verify-magic-link'
import { formatApiErrorMessage } from '@/lib/api/types'

function VerifyContent() {
  const searchParams = useSearchParams()
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const token = searchParams.get('token')

    if (!token) {
      setError('Link inválido ou expirado.')
      return
    }

    if (hasCompletedVerification(token)) {
      return
    }

    void verifyMagicLink(token)
      .then(async (login) => {
        if (hasCompletedVerification(token)) {
          return
        }

        markVerificationCompleted(token)
        establishSession(login)
        await rebootstrapSession()

        const destination = await resolvePostAuthRedirect(
          login,
          searchParams.get('returnUrl'),
        )

        if (isAbsoluteUrl(destination)) {
          window.location.assign(destination)
          return
        }

        window.location.replace(destination)
      })
      .catch((err) => {
        if (!hasCompletedVerification(token)) {
          setError(
            formatApiErrorMessage(err, 'Não foi possível validar o link.'),
          )
        }
      })
  }, [searchParams])

  if (error) {
    return (
      <div className="flex min-h-svh items-center justify-center px-4">
        <Card className="w-full max-w-md text-center">
          <CardHeader>
            <CardTitle>Não foi possível entrar</CardTitle>
            <CardDescription>{error}</CardDescription>
          </CardHeader>
          <CardContent>
            <Button asChild>
              <Link href="/auth">Voltar ao login</Link>
            </Button>
          </CardContent>
        </Card>
      </div>
    )
  }

  return <Loading fullPage label="Validando link de acesso..." />
}

export default function AuthVerifyPage() {
  return (
    <Suspense fallback={<Loading fullPage label="Validando link de acesso..." />}>
      <VerifyContent />
    </Suspense>
  )
}
