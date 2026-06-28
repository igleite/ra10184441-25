'use client'

import { useParams } from 'next/navigation'
import { useEffect, useState } from 'react'

import { EmptyState } from '@/components/shared/empty-state'
import { Loading } from '@/components/shared/loading'
import { PageHeader } from '@/components/shared/page-header'
import { TicketChatPanel } from '@/components/shared/ticket-chat-panel'
import { Label } from '@/components/ui/label'
import { apiGet, apiPost } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { useOrganizationTicketLabels } from '@/hooks/use-organization-ticket-labels'
import { usePlatformUserLabels } from '@/hooks/use-platform-user-labels'
import { useAuth } from '@/providers/auth-provider'
import { useCustomer } from '@/providers/customer-provider'
import { useTenant } from '@/providers/tenant-provider'

interface TicketDto {
  id: string
  customerId: string
  artifactId: string
  statusId: string
  classificationId: string
  description: string
  resolution: string
}

interface ChatDto {
  id: string
  userId: string
  message: string
  createdAt: string
}

const CHAT_PAGE_SIZE = 20

function ReadOnlyField({ label, value }: { label: string; value: string }) {
  return (
    <div className="space-y-2">
      <Label>{label}</Label>
      <p className="rounded-md border bg-muted/20 px-3 py-2 text-sm">{value}</p>
    </div>
  )
}

export default function PortalTicketDetailPage() {
  const params = useParams<{ id: string }>()
  const { organizationId } = useTenant()
  const { customerId } = useCustomer()
  const { user } = useAuth()
  const labels = useOrganizationTicketLabels(organizationId)
  const { userName } = usePlatformUserLabels()
  const [ticket, setTicket] = useState<TicketDto | null>(null)
  const [chats, setChats] = useState<ChatDto[]>([])
  const [chatMessage, setChatMessage] = useState('')
  const [chatReloadToken, setChatReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [loadedChatKey, setLoadedChatKey] = useState<string | null>(null)
  const [isSendingChat, setIsSendingChat] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [chatError, setChatError] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${params.id}-${customerId ?? 'none'}`
  const chatFetchKey = `${organizationId}-${params.id}-${chatReloadToken}`
  const isLoading = loadedKey !== fetchKey
  const isLoadingChats = loadedChatKey !== chatFetchKey

  useEffect(() => {
    let cancelled = false

    void apiGet<TicketDto>(`api/organizations/${organizationId}/tickets/${params.id}`)
      .then((loaded) => {
        if (cancelled) {
          return
        }

        if (customerId && loaded.customerId !== customerId) {
          setTicket(null)
          setError('Ticket não encontrado para o seu cliente.')
          setLoadedKey(fetchKey)
          return
        }

        setTicket(loaded)
        setError(null)
        setLoadedKey(fetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        setError(formatApiErrorMessage(err, 'Ticket não encontrado.'))
        setLoadedKey(fetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [customerId, fetchKey, organizationId, params.id])

  useEffect(() => {
    if (!ticket) {
      return
    }

    let cancelled = false

    void apiGet<PageDto<ChatDto>>(
      `api/organizations/${organizationId}/tickets/${params.id}/chats?pageIndex=1&pageSize=${CHAT_PAGE_SIZE}`,
    )
      .then((page) => {
        if (cancelled) {
          return
        }

        setChats(page.items ?? [])
        setChatError(null)
        setLoadedChatKey(chatFetchKey)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        if (err.statusCode === 404) {
          setChats([])
          setChatError(null)
          setLoadedChatKey(chatFetchKey)
          return
        }

        setChatError(formatApiErrorMessage(err, 'Erro ao carregar chat.'))
        setLoadedChatKey(chatFetchKey)
      })

    return () => {
      cancelled = true
    }
  }, [chatFetchKey, organizationId, params.id, ticket])

  async function handleSendMessage() {
    if (!ticket || !user?.userId) {
      setChatError('Usuário autenticado não encontrado.')
      return
    }

    if (!chatMessage.trim()) {
      return
    }

    setIsSendingChat(true)
    setChatError(null)

    try {
      await apiPost<ChatDto>(`api/organizations/${organizationId}/tickets/${ticket.id}/chats`, {
        userId: user.userId,
        message: chatMessage.trim(),
      })
      setChatMessage('')
      setChatReloadToken((value) => value + 1)
    } catch (err) {
      const apiError = err as ApiError
      setChatError(formatApiErrorMessage(apiError, 'Erro ao enviar mensagem.'))
    } finally {
      setIsSendingChat(false)
    }
  }

  if (isLoading || labels.isLoading) {
    return <Loading fullPage label="Carregando ticket..." />
  }

  if (!ticket) {
    return (
      <div className="space-y-8">
        <PageHeader
          title="Ticket não encontrado"
          breadcrumbs={[
            { label: 'Tickets', href: '/portal/tickets' },
            { label: 'Detalhe' },
          ]}
        />
        <EmptyState
          title="Ticket não encontrado"
          description={error ?? 'Não foi possível carregar o chamado.'}
          action={{ label: 'Voltar para tickets', href: '/portal/tickets' }}
        />
      </div>
    )
  }

  return (
    <div className="mx-auto max-w-3xl space-y-8">
      <PageHeader
        title={ticket.description}
        subtitle="Detalhes, mensagens e histórico do chamado."
        breadcrumbs={[
          { label: 'Tickets', href: '/portal/tickets' },
          { label: ticket.description },
        ]}
      />

      <section className="space-y-4 rounded-md border p-4">
        <h2 className="text-lg font-semibold">Informações</h2>
        <div className="grid gap-4 sm:grid-cols-2">
          <ReadOnlyField label="Status" value={labels.statusName(ticket.statusId)} />
          <ReadOnlyField
            label="Classificação"
            value={labels.classificationName(ticket.classificationId)}
          />
          <div className="sm:col-span-2">
            <ReadOnlyField label="Descrição" value={ticket.description} />
          </div>
          {ticket.resolution ? (
            <div className="sm:col-span-2">
              <ReadOnlyField label="Resolução" value={ticket.resolution} />
            </div>
          ) : null}
        </div>
      </section>

      <TicketChatPanel
        chats={chats}
        isLoading={isLoadingChats}
        message={chatMessage}
        onMessageChange={setChatMessage}
        onSubmit={handleSendMessage}
        isSubmitting={isSendingChat}
        error={chatError}
        resolveUserLabel={userName}
      />
    </div>
  )
}
