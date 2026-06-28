import Link from 'next/link'

import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'

export default async function ForbiddenPage({
  searchParams,
}: {
  searchParams: Promise<{ context?: string }>
}) {
  const params = await searchParams
  const backHref = params.context === 'tenant' ? '/auth' : '/'

  return (
    <div className="flex min-h-svh items-center justify-center px-4">
      <Card className="w-full max-w-md text-center">
        <CardHeader>
          <CardTitle>403 — Acesso negado</CardTitle>
          <CardDescription>
            Você não tem permissão para acessar esta área.
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <p className="text-sm text-muted-foreground">
            Verifique se está autenticado com o perfil correto ou se está no
            domínio adequado para esta rota.
          </p>
          <div className="flex flex-wrap justify-center gap-2">
            <Button asChild>
              <Link href={backHref}>Voltar</Link>
            </Button>
            <Button variant="outline" asChild>
              <Link href="/auth">Ir para login</Link>
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}
