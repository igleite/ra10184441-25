'use client'

import Link from 'next/link'
import { useEffect, useState } from 'react'

import { FormFeedback } from '@/components/shared/form-feedback'
import { PageHeader } from '@/components/shared/page-header'
import { Loading } from '@/components/shared/loading'
import { PermissionGate } from '@/components/shared/permission-gate'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { apiGet } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'

interface CountItem {
  id: string
}

export default function AdminDashboardPage() {
  const [counts, setCounts] = useState({
    organizations: 0,
    users: 0,
    featureFlags: 0,
  })
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    Promise.all([
      apiGet<PageDto<CountItem>>('api/organizations?pageIndex=1&pageSize=1'),
      apiGet<PageDto<CountItem>>('api/users?pageIndex=1&pageSize=1'),
      apiGet<PageDto<CountItem>>('api/feature-flags?pageIndex=1&pageSize=1'),
    ])
      .then(([organizations, users, featureFlags]) => {
        setCounts({
          organizations: organizations.totalItemCount,
          users: users.totalItemCount,
          featureFlags: featureFlags.totalItemCount,
        })
      })
      .catch((err: ApiError) => {
        setError(formatApiErrorMessage(err, 'Erro ao carregar dashboard.'))
      })
      .finally(() => {
        setIsLoading(false)
      })
  }, [])

  if (isLoading) {
    return <Loading fullPage label="Carregando dashboard..." />
  }

  return (
    <div className="space-y-8">
      <PageHeader
        title="Dashboard da plataforma"
        subtitle="Visão operacional global para SuperAdmin."
      />

      {error ? <FormFeedback error={error} /> : null}

      <div className="grid gap-4 sm:grid-cols-3">
        <PermissionGate action="admin:organizations">
          <Card>
            <CardHeader>
              <CardTitle className="text-2xl">{counts.organizations}</CardTitle>
              <CardDescription>Organizações</CardDescription>
            </CardHeader>
            <CardContent>
              <Button variant="outline" size="sm" asChild>
                <Link href="/admin/organizations">Gerenciar</Link>
              </Button>
            </CardContent>
          </Card>
        </PermissionGate>
        <PermissionGate action="admin:users">
          <Card>
            <CardHeader>
              <CardTitle className="text-2xl">{counts.users}</CardTitle>
              <CardDescription>Usuários globais</CardDescription>
            </CardHeader>
            <CardContent>
              <Button variant="outline" size="sm" asChild>
                <Link href="/admin/users">Gerenciar</Link>
              </Button>
            </CardContent>
          </Card>
        </PermissionGate>
        <PermissionGate action="admin:feature-flags">
          <Card>
            <CardHeader>
              <CardTitle className="text-2xl">{counts.featureFlags}</CardTitle>
              <CardDescription>Feature flags</CardDescription>
            </CardHeader>
            <CardContent>
              <Button variant="outline" size="sm" asChild>
                <Link href="/admin/feature-flags">Gerenciar</Link>
              </Button>
            </CardContent>
          </Card>
        </PermissionGate>
      </div>
    </div>
  )
}
