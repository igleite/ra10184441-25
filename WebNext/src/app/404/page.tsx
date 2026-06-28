import Link from 'next/link'

import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'

export default async function NotFoundPage({
  searchParams,
}: {
  searchParams: Promise<{ reason?: string }>
}) {
  const params = await searchParams
  const isInvalidSlug = params.reason === 'invalid-slug'

  return (
    <div className="flex min-h-svh items-center justify-center px-4">
      <Card className="w-full max-w-md text-center">
        <CardHeader>
          <CardTitle>404 — Não encontrado</CardTitle>
          <CardDescription>
            {isInvalidSlug
              ? 'Organização não encontrada para este subdomínio.'
              : 'O recurso solicitado não existe ou foi removido.'}
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <p className="text-sm text-muted-foreground">
            {isInvalidSlug
              ? 'Confira o endereço da organização ou entre em contato com o administrador.'
              : 'Verifique o endereço digitado ou retorne à página inicial.'}
          </p>
          <div className="flex flex-wrap justify-center gap-2">
            <Button asChild>
              <Link href="/">Página inicial</Link>
            </Button>
            {!isInvalidSlug ? (
              <Button variant="outline" asChild>
                <Link href="/auth">Login</Link>
              </Button>
            ) : null}
          </div>
        </CardContent>
      </Card>
    </div>
  )
}
