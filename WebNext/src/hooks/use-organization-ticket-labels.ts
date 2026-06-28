'use client'

import { useEffect, useMemo, useState } from 'react'

import { apiGet } from '@/lib/api/client'
import type { PageDto } from '@/lib/api/types'

interface CustomerDto {
  id: string
  name: string
}

interface StatusReasonDto {
  id: string
  name: string
}

interface TicketClassificationDto {
  id: string
  name: string
}

const LOOKUP_PAGE_SIZE = 100

export function useOrganizationTicketLabels(organizationId: string) {
  const [customerLabels, setCustomerLabels] = useState<Map<string, string>>(new Map())
  const [statusLabels, setStatusLabels] = useState<Map<string, string>>(new Map())
  const [classificationLabels, setClassificationLabels] = useState<Map<string, string>>(new Map())
  const [loadedKey, setLoadedKey] = useState<string | null>(null)
  const isLoading = loadedKey !== organizationId

  useEffect(() => {
    let cancelled = false

    async function load() {
      const emptyPage = <T,>() =>
        ({ items: [], totalItemCount: 0, pageIndex: 1, pageSize: LOOKUP_PAGE_SIZE }) satisfies PageDto<T>

      const [customersPage, statusPage, classificationsPage] = await Promise.all([
        apiGet<PageDto<CustomerDto>>(
          `api/organizations/${organizationId}/customers?pageIndex=1&pageSize=${LOOKUP_PAGE_SIZE}`,
        ).catch(() => emptyPage<CustomerDto>()),
        apiGet<PageDto<StatusReasonDto>>(
          `api/organizations/${organizationId}/ticket-status-reasons?pageIndex=1&pageSize=${LOOKUP_PAGE_SIZE}`,
        ).catch(() => emptyPage<StatusReasonDto>()),
        apiGet<PageDto<TicketClassificationDto>>(
          `api/organizations/${organizationId}/ticket-classifications?pageIndex=1&pageSize=${LOOKUP_PAGE_SIZE}`,
        ).catch(() => emptyPage<TicketClassificationDto>()),
      ])

      if (cancelled) {
        return
      }

      setCustomerLabels(
        new Map((customersPage.items ?? []).map((item) => [item.id, item.name])),
      )
      setStatusLabels(
        new Map((statusPage.items ?? []).map((item) => [item.id, item.name])),
      )
      setClassificationLabels(
        new Map((classificationsPage.items ?? []).map((item) => [item.id, item.name])),
      )
      setLoadedKey(organizationId)
    }

    void load()

    return () => {
      cancelled = true
    }
  }, [organizationId])

  const helpers = useMemo(
    () => ({
      customerName: (id: string) => customerLabels.get(id) ?? id,
      statusName: (id: string) => statusLabels.get(id) ?? id,
      classificationName: (id: string) => classificationLabels.get(id) ?? id,
    }),
    [classificationLabels, customerLabels, statusLabels],
  )

  return {
    isLoading,
    customerLabels,
    statusLabels,
    classificationLabels,
    ...helpers,
  }
}
