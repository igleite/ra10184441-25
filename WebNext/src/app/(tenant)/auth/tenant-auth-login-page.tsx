'use client'

import { useSearchParams } from 'next/navigation'
import { useCallback, useMemo } from 'react'

import { MagicLinkLoginForm } from '@/components/auth/magic-link-login-form'
import { setPendingReturnUrl } from '@/lib/auth/pending-return-url'
import { useOrganizationBySlug } from '@/hooks/use-organization-by-slug'

function normalizeReturnPath(value: string | null): string {
  if (!value) {
    return '/app'
  }

  const path = value.startsWith('/') ? value : `/${value}`
  if (!path.startsWith('/app') && !path.startsWith('/portal')) {
    return '/app'
  }

  return path
}

export default function TenantAuthLoginPage() {
  const searchParams = useSearchParams()
  const { organization, slug, isLoading } = useOrganizationBySlug()
  const tenantLabel = organization?.name ?? slug

  const returnPath = useMemo(
    () => normalizeReturnPath(searchParams.get('returnPath')),
    [searchParams],
  )

  const handleBeforeLogin = useCallback(() => {
    const returnUrl = `${window.location.origin}${returnPath}`
    setPendingReturnUrl(returnUrl)
  }, [returnPath])

  const description = slug ? (
    <>
      Equipe interna e portal do cliente — subdomínio{' '}
      <span className="font-medium">{slug}</span>.
    </>
  ) : isLoading ? (
    'Carregando organização...'
  ) : (
    'Equipe interna e portal do cliente.'
  )

  const successDescription = slug ? (
    <>
      O link abrirá no mesmo endereço. Após confirmar o acesso, você voltará
      para esta organização.
    </>
  ) : (
    'O link abrirá no mesmo endereço. Após confirmar o acesso, você voltará para esta organização.'
  )

  return (
    <MagicLinkLoginForm
      title={tenantLabel ? `Entrar em ${tenantLabel}` : 'Entrar'}
      description={description}
      successDescription={successDescription}
      registerHref="/register"
      onBeforeLogin={handleBeforeLogin}
    />
  )
}
