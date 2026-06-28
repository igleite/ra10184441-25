'use client'

import { useRouter, useSearchParams } from 'next/navigation'
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
import { apiGet } from '@/lib/api/client'
import { useAuth } from '@/providers/auth-provider'
import type { OrganizationDto } from '@/types/auth'

function NoAccessContent() {
  const router = useRouter()
  const searchParams = useSearchParams()
  const { logout } = useAuth()
  const slug = searchParams.get('slug') ?? ''
  const [organizationName, setOrganizationName] = useState<string | null>(null)

  useEffect(() => {
    if (!slug) {
      return
    }

    let cancelled = false

    void apiGet<OrganizationDto>(
      `api/organizations/slug/${encodeURIComponent(slug)}`,
    )
      .then((org) => {
        if (!cancelled) {
          setOrganizationName(org.name)
        }
      })
      .catch(() => {
        if (!cancelled) {
          setOrganizationName(null)
        }
      })

    return () => {
      cancelled = true
    }
  }, [slug])

  const orgLabel = organizationName ?? slug

  return (
    <div className="flex min-h-svh items-center justify-center px-4">
      <Card className="w-full max-w-md text-center">
        <CardHeader>
          <CardTitle>Sem acesso</CardTitle>
          <CardDescription>
            {orgLabel
              ? `Sua conta não tem permissão para acessar ${orgLabel}.`
              : 'Sua conta não tem permissão para acessar esta organização.'}
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <p className="text-sm text-muted-foreground">
            {slug ? (
              <>
                Subdomínio <span className="font-medium">{slug}</span>. Verifique
                se está usando o e-mail correto ou peça acesso ao administrador
                da organização.
              </>
            ) : (
              'Verifique se está usando o e-mail correto ou peça acesso ao administrador da organização.'
            )}
          </p>
          <div className="flex flex-wrap justify-center gap-2">
            <Button type="button" variant="outline" onClick={() => router.back()}>
              Voltar
            </Button>
            <Button
              type="button"
              onClick={() => {
                void logout()
              }}
            >
              Entrar com outra conta
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

export default function NoAccessPage() {
  return (
    <Suspense fallback={<Loading fullPage label="Carregando..." />}>
      <NoAccessContent />
    </Suspense>
  )
}
