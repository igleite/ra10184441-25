'use client'

import Link from 'next/link'
import { useEffect, useState } from 'react'

import { EmptyState } from '@/components/shared/empty-state'
import { FormFeedback } from '@/components/shared/form-feedback'
import { Loading } from '@/components/shared/loading'
import { PageHeader } from '@/components/shared/page-header'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { apiGet } from '@/lib/api/client'
import { filterMyOrganizations } from '@/lib/account-organizations'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { buildTenantUrl } from '@/lib/tenant'
import { useAuth } from '@/providers/auth-provider'

interface OrganizationDto {
  id: string
  name: string
  document: string
  slug: string
}

export default function AccountDashboardPage() {
  const { user } = useAuth()
  const [organizations, setOrganizations] = useState<OrganizationDto[]>([])
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)
  const fetchKey = user?.userId ?? 'anonymous'
  const isLoading = loadedKey !== fetchKey

  useEffect(() => {
    if (!user?.userId) {
      return
    }

    let cancelled = false

    void apiGet<PageDto<OrganizationDto>>('api/organizations?pageIndex=1&pageSize=100')
      .then(async (page) => {
        if (cancelled) {
          return
        }

        const mine = await filterMyOrganizations(page.items ?? [], user.userId)
        setOrganizations(mine)
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        if (err.statusCode === 404) {
          setOrganizations([])
          setError(null)
          setLoadedKey(fetchKey)
          return
        }

        setError(formatApiErrorMessage(err, 'Erro ao carregar suas organizações.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [fetchKey, user?.userId])

  if (isLoading) {
    return <Loading fullPage label="Carregando dashboard..." />
  }

  return (
    <div className="space-y-8">
      <PageHeader
        title="Minha conta"
        subtitle="Gerencie suas organizações e acesse o ambiente operacional de cada uma."
        actions={
          <div className="flex gap-2">
            <Button variant="outline" asChild>
              <Link href="/account/profile">Perfil</Link>
            </Button>
            <Button asChild>
              <Link href="/account/organizations/new">Nova organização</Link>
            </Button>
          </div>
        }
      />

      {error ? <FormFeedback error={error} /> : null}

      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">{organizations.length}</CardTitle>
          <CardDescription>Organizações vinculadas à sua conta</CardDescription>
        </CardHeader>
        <CardContent>
          <Button variant="outline" size="sm" asChild>
            <Link href="/account/organizations">Ver todas</Link>
          </Button>
        </CardContent>
      </Card>

      {organizations.length === 0 ? (
        <EmptyState
          title="Nenhuma organização ainda"
          description="Crie sua primeira organização para acessar o app pelo subdomínio."
          action={{ label: 'Criar organização', href: '/account/organizations/new' }}
        />
      ) : (
        <div className="grid gap-4 md:grid-cols-2">
          {organizations.map((organization) => (
            <Card key={organization.id}>
              <CardHeader>
                <CardTitle className="text-base">{organization.name}</CardTitle>
                <CardDescription>{organization.slug}</CardDescription>
              </CardHeader>
              <CardContent className="flex flex-wrap gap-2">
                <Button size="sm" asChild>
                  <a
                    href={buildTenantUrl(organization.slug, '/app')}
                    target="_blank"
                    rel="noreferrer"
                  >
                    Abrir app
                  </a>
                </Button>
                <Button variant="outline" size="sm" asChild>
                  <Link href={`/account/organizations/${organization.id}`}>Detalhes</Link>
                </Button>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  )
}
