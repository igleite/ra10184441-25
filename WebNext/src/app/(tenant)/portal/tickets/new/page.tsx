'use client'

import { useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { CustomerArtifactPicker } from '@/components/shared/customer-artifact-picker'
import { EmptyState } from '@/components/shared/empty-state'
import { FormContainer } from '@/components/shared/form-container'
import { Loading } from '@/components/shared/loading'
import { PageHeader } from '@/components/shared/page-header'
import { SelectField } from '@/components/shared/select-field'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiGet, apiPost } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { useAuth } from '@/providers/auth-provider'
import { useCustomer } from '@/providers/customer-provider'
import { useTenant } from '@/providers/tenant-provider'

interface TicketDto {
  id: string
}

interface TicketClassificationDto {
  id: string
  name: string
}

const OPTIONS_PAGE_SIZE = 50

export default function PortalTicketsNewPage() {
  const router = useRouter()
  const { organizationId } = useTenant()
  const { customerId } = useCustomer()
  const { user } = useAuth()
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [classifications, setClassifications] = useState<TicketClassificationDto[]>([])
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const fetchKey = `${organizationId}-${customerId ?? 'none'}`
  const isLoading = customerId ? loadedKey !== fetchKey : false
  const [form, setForm] = useState({
    artifactId: '',
    classificationId: '',
    description: '',
  })

  useEffect(() => {
    if (!customerId) {
      return
    }

    let cancelled = false

    async function loadOptions() {
      try {
        const classificationsPage = await apiGet<PageDto<TicketClassificationDto>>(
          `api/organizations/${organizationId}/ticket-classifications?pageIndex=1&pageSize=${OPTIONS_PAGE_SIZE}`,
        )

        if (cancelled) {
          return
        }

        setClassifications(classificationsPage.items ?? [])
        setLoadedKey(fetchKey)
      } catch (err) {
        if (cancelled) {
          return
        }

        const apiError = err as ApiError
        setError(formatApiErrorMessage(apiError, 'Erro ao carregar dados do formulário.'))
        setLoadedKey(fetchKey)
      }
    }

    void loadOptions()

    return () => {
      cancelled = true
    }
  }, [customerId, fetchKey, organizationId])

  async function handleSubmit() {
    if (!customerId) {
      setError('Cliente não identificado no token.')
      return
    }

    if (!user?.userId) {
      setError('Usuário autenticado não encontrado.')
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const created = await apiPost<TicketDto>(`api/organizations/${organizationId}/tickets`, {
        customerId,
        artifactId: form.artifactId,
        classificationId: form.classificationId,
        createdByUserId: user.userId,
        description: form.description.trim(),
      })
      router.push(`/portal/tickets/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar ticket.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  if (!customerId) {
    return (
      <div className="space-y-8">
        <PageHeader
          title="Novo ticket"
          subtitle="Abra um chamado para o seu cliente."
          breadcrumbs={[
            { label: 'Tickets', href: '/portal/tickets' },
            { label: 'Novo' },
          ]}
        />
        <EmptyState
          title="Cliente não identificado"
          description="O customerId do token é necessário para abrir um ticket."
        />
      </div>
    )
  }

  if (isLoading) {
    return <Loading fullPage label="Carregando formulário..." />
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title="Novo ticket"
        subtitle="Formulário simplificado — cliente implícito do token."
        breadcrumbs={[
          { label: 'Tickets', href: '/portal/tickets' },
          { label: 'Novo' },
        ]}
      />

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        submitLabel="Criar ticket"
        cancelHref="/portal/tickets"
        error={error}
      >
        <div className="space-y-4">
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
            placeholder="Selecione uma classificação"
            required
          />

          <CustomerArtifactPicker
            organizationId={organizationId}
            customerId={customerId}
            value={form.artifactId}
            onChange={(value) => setForm((prev) => ({ ...prev, artifactId: value }))}
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
        </div>
      </FormContainer>
    </div>
  )
}
