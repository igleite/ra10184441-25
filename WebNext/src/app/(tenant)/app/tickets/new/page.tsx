'use client'

import { useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'

import { CustomerArtifactPicker } from '@/components/shared/customer-artifact-picker'
import { FormContainer } from '@/components/shared/form-container'
import { Loading } from '@/components/shared/loading'
import { PageHeader } from '@/components/shared/page-header'
import { SelectField } from '@/components/shared/select-field'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { apiGet, apiPost } from '@/lib/api/client'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'
import { useAuth } from '@/providers/auth-provider'
import { useTenant } from '@/providers/tenant-provider'

interface TicketDto {
  id: string
}

interface CustomerDto {
  id: string
  name: string
}

interface TicketClassificationDto {
  id: string
  name: string
}

const OPTIONS_PAGE_SIZE = 50

export default function AppTicketsNewPage() {
  const router = useRouter()
  const { organizationId } = useTenant()
  const { user } = useAuth()
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [customers, setCustomers] = useState<CustomerDto[]>([])
  const [classifications, setClassifications] = useState<TicketClassificationDto[]>([])
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const fetchKey = organizationId
  const isLoading = loadedKey !== fetchKey
  const [form, setForm] = useState({
    customerId: '',
    artifactId: '',
    classificationId: '',
    description: '',
  })

  useEffect(() => {
    let cancelled = false

    async function loadOptions() {
      try {
        const [customersPage, classificationsPage] = await Promise.all([
          apiGet<PageDto<CustomerDto>>(
            `api/organizations/${organizationId}/customers?pageIndex=1&pageSize=${OPTIONS_PAGE_SIZE}`,
          ),
          apiGet<PageDto<TicketClassificationDto>>(
            `api/organizations/${organizationId}/ticket-classifications?pageIndex=1&pageSize=${OPTIONS_PAGE_SIZE}`,
          ),
        ])

        if (cancelled) {
          return
        }

        setCustomers(customersPage.items ?? [])
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
  }, [fetchKey, organizationId])

  async function handleSubmit() {
    if (!user?.userId) {
      setError('Usuário autenticado não encontrado.')
      return
    }

    setIsSubmitting(true)
    setError(null)

    try {
      const created = await apiPost<TicketDto>(`api/organizations/${organizationId}/tickets`, {
        customerId: form.customerId,
        artifactId: form.artifactId,
        classificationId: form.classificationId,
        createdByUserId: user.userId,
        description: form.description.trim(),
      })
      router.push(`/app/tickets/${created.id}`)
    } catch (err) {
      const apiError = err as ApiError
      setError(formatApiErrorMessage(apiError, 'Erro ao criar ticket.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  if (isLoading) {
    return <Loading fullPage label="Carregando formulário..." />
  }

  return (
    <div className="mx-auto max-w-2xl space-y-8">
      <PageHeader
        title="Novo ticket"
        subtitle="Abra um chamado para atendimento interno."
        breadcrumbs={[
          { label: 'Tickets', href: '/app/tickets' },
          { label: 'Novo' },
        ]}
      />

      <FormContainer
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        submitLabel="Criar ticket"
        cancelHref="/app/tickets"
        error={error}
      >
        <div className="space-y-4">
          <SelectField
            id="customerId"
            label="Cliente"
            value={form.customerId}
            onChange={(value) =>
              setForm((prev) => ({ ...prev, customerId: value, artifactId: '' }))
            }
            options={customers.map((customer) => ({
              value: customer.id,
              label: customer.name,
            }))}
            placeholder="Selecione um cliente"
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
            placeholder="Selecione uma classificação"
            required
          />

          {form.customerId ? (
            <CustomerArtifactPicker
              organizationId={organizationId}
              customerId={form.customerId}
              value={form.artifactId}
              onChange={(value) => setForm((prev) => ({ ...prev, artifactId: value }))}
              required
            />
          ) : (
            <p className="text-sm text-muted-foreground">
              Selecione um cliente para escolher o artefato vinculado.
            </p>
          )}

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
