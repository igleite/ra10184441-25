import Link from 'next/link'

import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'

const benefits = [
  {
    title: 'Cadastro de animais',
    description:
      'Gerencie cães, gatos, papagaios, coelhos e outros animais em um único sistema.',
  },
  {
    title: 'Histórico completo',
    description:
      'Consulte rapidamente consultas, exames, vacinas e cirurgias realizadas.',
  },
  {
    title: 'Gestão da clínica',
    description:
      'Centralize atendimentos, equipe veterinária e informações dos tutores.',
  },
]

export default function HomePage() {
  return (
    <div className="min-h-svh bg-gradient-to-b from-background to-muted/30">
      <header className="mx-auto flex max-w-6xl items-center justify-between px-6 py-6">
        <div>
          <p className="text-sm font-medium">Plataforma de Gestão Veterinária</p>
          <p className="text-xs text-muted-foreground">
            Cadastro de animais e histórico de atendimentos
          </p>
        </div>
        <div className="flex gap-2">
          <Button variant="outline" asChild>
            <Link href="/auth">Entrar</Link>
          </Button>
          <Button asChild>
            <Link href="/register">Criar conta</Link>
          </Button>
        </div>
      </header>

      <main className="mx-auto max-w-6xl px-6 pb-16">
        <section className="py-16 text-center">
          <h1 className="text-4xl font-semibold tracking-tight sm:text-5xl">
            Gestão completa para clínicas veterinárias
          </h1>
          <p className="mx-auto mt-4 max-w-2xl text-muted-foreground">
            Centralize o atendimento dos seus pacientes em um único lugar.
            Acompanhe o histórico de cada animal — cachorro, gato, papagaio ou
            coelho — registre consultas, exames, cirurgias e vacinas, e consulte
            todos os serviços realizados ao longo da vida do paciente.
          </p>
          <div className="mt-8 flex flex-wrap justify-center gap-3">
            <Button size="lg" asChild>
              <Link href="/register">Começar agora</Link>
            </Button>
            <Button size="lg" variant="outline" asChild>
              <Link href="/auth">Já tenho conta</Link>
            </Button>
          </div>
        </section>

        <section className="grid gap-4 md:grid-cols-3">
          {benefits.map((benefit) => (
            <Card key={benefit.title}>
              <CardHeader>
                <CardTitle className="text-base">{benefit.title}</CardTitle>
              </CardHeader>
              <CardContent>
                <CardDescription>{benefit.description}</CardDescription>
              </CardContent>
            </Card>
          ))}
        </section>

        <section className="mt-16 rounded-xl border bg-card p-8 text-center">
          <h2 className="text-xl font-semibold">Tudo em um só lugar</h2>
          <p className="mx-auto mt-2 max-w-xl text-sm text-muted-foreground">
            Controle pacientes, tutores e serviços realizados através de uma
            interface moderna e simples de utilizar. Cadastre clínicas, animais
            e tutores, e mantenha o histórico completo de consultas, exames,
            cirurgias e vacinas de cada paciente.
          </p>
        </section>
      </main>
    </div>
  )
}
