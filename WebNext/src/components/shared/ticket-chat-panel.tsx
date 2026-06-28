'use client'

import { EmptyState } from '@/components/shared/empty-state'
import { FormContainer } from '@/components/shared/form-container'
import { Loading } from '@/components/shared/loading'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

export interface TicketChatMessage {
  id: string
  userId: string
  message: string
  createdAt: string
}

export interface TicketChatPanelProps {
  chats: TicketChatMessage[]
  isLoading: boolean
  message: string
  onMessageChange: (value: string) => void
  onSubmit: () => void | Promise<void>
  isSubmitting: boolean
  error?: string | null
  resolveUserLabel?: (userId: string) => string
}

export function TicketChatPanel({
  chats,
  isLoading,
  message,
  onMessageChange,
  onSubmit,
  isSubmitting,
  error,
  resolveUserLabel = (userId) => userId,
}: TicketChatPanelProps) {
  return (
    <section className="space-y-4" aria-label="Chat do ticket">
      <div>
        <h2 className="text-lg font-semibold">Chat</h2>
        <p className="text-sm text-muted-foreground">Mensagens vinculadas ao ticket.</p>
      </div>

      <div className="space-y-3 rounded-md border p-4">
        {isLoading ? (
          <Loading label="Carregando mensagens..." />
        ) : chats.length === 0 ? (
          <EmptyState
            title="Nenhuma mensagem"
            description="Envie a primeira mensagem para iniciar a conversa."
          />
        ) : (
          chats.map((chat) => (
            <div key={chat.id} className="rounded-md border bg-muted/20 p-3">
              <p className="text-xs text-muted-foreground">
                {resolveUserLabel(chat.userId)} —{' '}
                {new Date(chat.createdAt).toLocaleString('pt-BR')}
              </p>
              <p className="mt-1 text-sm">{chat.message}</p>
            </div>
          ))
        )}
      </div>

      <FormContainer
        onSubmit={onSubmit}
        isSubmitting={isSubmitting}
        submitLabel="Enviar mensagem"
        error={error}
      >
        <div className="space-y-2">
          <Label htmlFor="chatMessage">Nova mensagem</Label>
          <Input
            id="chatMessage"
            value={message}
            onChange={(event) => onMessageChange(event.target.value)}
            placeholder="Digite a mensagem para o ticket"
            required
          />
        </div>
      </FormContainer>
    </section>
  )
}
