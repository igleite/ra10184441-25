'use client'

import { MagicLinkLoginForm } from '@/components/auth/magic-link-login-form'
import { clearPendingReturnUrl } from '@/lib/auth/pending-return-url'

export function PublicAuthLoginPage() {
  return (
    <MagicLinkLoginForm
      title="Entrar na plataforma"
      description="Acesso para administradores da plataforma e donos de organizações."
      successDescription="O link abrirá neste site (domínio raiz). Após confirmar o acesso, você será redirecionado automaticamente."
      onBeforeLogin={clearPendingReturnUrl}
    />
  )
}
