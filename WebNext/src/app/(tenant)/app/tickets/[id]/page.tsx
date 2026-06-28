'use client'

import { useParams } from 'next/navigation'
import { useEffect, useState } from 'react'

import { CustomerArtifactPicker } from '@/components/shared/customer-artifact-picker'
import { EmptyState } from '@/components/shared/empty-state'
import { FormContainer } from '@/components/shared/form-container'
import { Loading } from '@/components/shared/loading'
import { PageHeader } from '@/components/shared/page-header'
import { SelectField } from '@/components/shared/select-field'
import { TicketChatPanel } from '@/components/shared/ticket-chat-panel'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiGet, apiPost, apiPut } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { ALLOCATION_CENTER_OPTIONS } from '@/lib/allocation-center'
import { usePlatformUserLabels } from '@/hooks/use-platform-user-labels'
import { useAuth } from '@/providers/auth-provider'
import { useTenant } from '@/providers/tenant-provider'

interface TicketDto {
  id: string
  customerId: string
  artifactId: string
  statusId: string
  classificationId: string
  allocationCenter: number
  description: string
  resolution: string
  rowVersion: string
}

interface ChatDto {
  id: string
  userId: string
  message: string
  createdAt: string
}

interface StatusReasonDto {
  id: string
  name: string
}

interface TicketClassificationDto {
  id: string
  name: string
}

const CHAT_PAGE_SIZE = 20
const OPTIONS_PAGE_SIZE = 100

export default function AppTicketDetailPage() {
  const params = useParams<{ id: string }>()
  const { organizationId } = useTenant()
  const { user } = useAuth()
  const { userName } = usePlatformUserLabels()
  const [ticket, setTicket] = useState<TicketDto | null>(null)
  const [form, setForm] = useState({
    statusId: '',
    classificationId: '',
    artifactId: '',
    allocationCenter: '0',
    description: '',
    resolution: '',
  })
  const [chats, setChats] = useState<ChatDto[]>([])
  const [chatMessage, setChatMessage] = useState('')
  const [chatReloadToken, setChatReloadToken] = useState(0)
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const [loadedChatKey, setLoadedChatKey] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isSendingChat, setIsSendingChat] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [chatError, setChatError] = useState<string | null>(null)
  const [saved, setSaved] = useState(false)
  const [statusReasons, setStatusReasons] = useState<StatusReasonDto[]>([])
  const [classifications, setClassifications] = useState<TicketClassificationDto[]>([])
  const fetchKey = `${organizationId}-${params.id}`
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

        setTicket(loaded)
        setForm({
          statusId: loaded.statusId,
          classificationId: loaded.classificationId,
          artifactId: loaded.artifactId,
          allocationCenter: String(loaded.allocationCenter),
          description: loaded.description,
          resolution: loaded.resolution,
        })
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
  }, [fetchKey, organizationId, params.id])

  useEffect(() => {
    let cancelled = false

    void Promise.all([
      apiGet<PageDto<StatusReasonDto>>(
        `api/organizations/${organizationId}/ticket-status-reasons?pageIndex=1&pageSize=${OPTIONS_PAGE_SIZE}`,
      ),
      apiGet<PageDto<TicketClassificationDto>>(
        `api/organizations/${organizationId}/ticket-classifications?pageIndex=1&pageSize=${OPTIONS_PAGE_SIZE}`,
      ),
    ])
      .then(([statusPage, classificationsPage]) => {
        if (!cancelled) {
          setStatusReasons(statusPage.items ?? [])
          setClassifications(classificationsPage.items ?? [])
        }
      })
      .catch(() => {
        if (!cancelled) {
          setStatusReasons([])
          setClassifications([])
        }
      })

    return () => {
      cancelled = true
    }
  }, [organizationId])

  useEffect(() => {
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
  }, [chatFetchKey, organizationId, params.id])

  async function handleSubmit() {
    if (!ticket) {
      return
    }

    setIsSubmitting(true)
    setError(null)
    setSaved(false)

    try {
      const updated = await apiPut<TicketDto>(`api/organizations/${organizationId}/tickets/${ticket.id}`, {
        statusId: form.statusId,
        classificationId: form.classificationId,
        artifactId: form.artifactId,
        allocationCenter: Number(form.allocationCenter),
        description: form.description.trim(),
        resolution: form.resolution.trim(),
        rowVersion: ticket.rowVersion,
      })
      setTicket(updated)
      setForm({
        statusId: updated.statusId,
        classificationId: updated.classificationId,
        artifactId: updated.artifactId,
        allocationCenter: String(updated.allocationCenter),
        description: updated.description,
        resolution: updated.resolution,
      })
      setSaved(true)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao salvar ticket.'))
    } finally {
      setIsSubmitting(false)
    }
  }

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

  if (isLoading) {
    return <Loading fullPage label="Carregando ticket..." />
  }

  if (!ticket) {
    return (
      <EmptyState
        title="Ticket não encontrado"
        description={error ?? 'Não foi possível carregar o chamado.'}
        action={{ label: 'Voltar para tickets', href: '/app/tickets' }}
      />
    )
  }

  return (
    <div className="mx-auto max-w-3xl space-y-8">
      <PageHeader
        title={`Ticket ${ticket.id}`}
        subtitle="Detalhes, atualização de status e histórico de conversa."
        breadcrumbs={[
          { label: 'Tickets', href: '/app/tickets' },
          { label: ticket.id },
        ]}
      />

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        submitLabel="Salvar alterações"
        cancelHref="/app/tickets"
        error={error}
        success={saved ? 'Alterações salvas com sucesso.' : null}
      >
        <div className="space-y-4">
          <SelectField
            id="statusId"
            label="Status"
            value={form.statusId}
            onChange={(value) => setForm((prev) => ({ ...prev, statusId: value }))}
            options={statusReasons.map((status) => ({
              value: status.id,
              label: status.name,
            }))}
            placeholder="Selecione o status"
            required
          />
          <SelectField
            id="classificationId"
            label="Classificação"
            value={form.classificationId}
            onChange={(value) =>
              setForm((prev) => ({ ...prev, classificationId: value }))
            }
            options={classifications.map((classification) => ({
              value: classification.id,
              label: classification.name,
            }))}
            placeholder="Selecione a classificação"
            required
          />
          <CustomerArtifactPicker
            organizationId={organizationId}
            customerId={ticket.customerId}
            value={form.artifactId}
            onChange={(value) => setForm((prev) => ({ ...prev, artifactId: value }))}
            required
          />
          <SelectField
            id="allocationCenter"
            label="Centro de alocação"
            value={form.allocationCenter}
            onChange={(value) =>
              setForm((prev) => ({ ...prev, allocationCenter: value }))
            }
            options={[...ALLOCATION_CENTER_OPTIONS]}
            placeholder="Selecione..."
            required
          />
          <div className="space-y-2">
            <Label htmlFor="description">Descrição</Label>
            <Input
              id="description"
              value={form.description}
              onChange={(event) =>
                setForm((prev) => ({ ...prev, description: event.target.value }))
              }
              required
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="resolution">Resolução</Label>
            <Input
              id="resolution"
              value={form.resolution}
              onChange={(event) =>
                setForm((prev) => ({ ...prev, resolution: event.target.value }))
              }
              required
            />
          </div>
        </div>
      </FormContainer>

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
