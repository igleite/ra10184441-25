'use client'

import { useEffect, useMemo, useState } from 'react'

import { SearchDialog } from '@/components/shared/search-dialog'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Loading } from '@/components/shared/loading'
import { apiGet } from '@/lib/api/client'
import { filterListByQuery } from '@/lib/filter-list'
import { formatApiErrorMessage, type ApiError, type PageDto } from '@/lib/api/types'

export interface EntitySearchItem {
  id: string
  label: string
  description?: string
}

export interface EntitySearchDialogProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  title: string
  description?: string
  endpoint: string
  mapItem: (raw: Record<string, unknown>) => EntitySearchItem | null
  searchFields?: string[]
  onSelect: (item: EntitySearchItem) => void
}

export function EntitySearchDialog({
  open,
  onOpenChange,
  title,
  description,
  endpoint,
  mapItem,
  searchFields = ['label', 'description'],
  onSelect,
}: EntitySearchDialogProps) {
  const [query, setQuery] = useState('')
  const [items, setItems] = useState<EntitySearchItem[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<Record<string, unknown>>>(endpoint)
      .then((page) => {
        if (cancelled) {
          return
        }

        const mapped = (page.items ?? [])
          .map((item) => mapItem(item))
          .filter((item): item is EntitySearchItem => item !== null)
        setItems(mapped)
        setError(null)
      })
      .catch((err: ApiError) => {
        if (cancelled) {
          return
        }

        setError(formatApiErrorMessage(err, 'Erro ao carregar opções.'))
        setItems([])
      })
      .finally(() => {
        if (!cancelled) {
          setIsLoading(false)
        }
      })

    return () => {
      cancelled = true
    }
  }, [endpoint, mapItem])

  const filtered = useMemo(
    () =>
      filterListByQuery(
        items.map((item) => ({
          ...item,
          label: item.label,
          description: item.description ?? '',
        })),
        query,
        searchFields as ('label' | 'description' | 'id')[],
      ),
    [items, query, searchFields],
  )

  if (!open) {
    return null
  }

  return (
    <SearchDialog open={open} onOpenChange={onOpenChange} title={title} description={description}>
      <div className="space-y-4">
        <Input
          type="search"
          value={query}
          onChange={(event) => setQuery(event.target.value)}
          placeholder="Buscar..."
          autoFocus
        />
        {error ? <p className="text-sm text-destructive">{error}</p> : null}
        {isLoading ? <Loading label="Carregando..." /> : null}
        {!isLoading && filtered.length === 0 ? (
          <p className="text-sm text-muted-foreground">Nenhum resultado encontrado.</p>
        ) : null}
        {!isLoading ? (
          <ul className="max-h-64 space-y-2 overflow-y-auto">
            {filtered.map((item) => (
              <li key={item.id}>
                <Button
                  type="button"
                  variant="outline"
                  className="h-auto w-full justify-start px-3 py-2 text-left"
                  onClick={() => {
                    onSelect(item)
                    onOpenChange(false)
                    setQuery('')
                  }}
                >
                  <span className="block font-medium">{item.label}</span>
                  {item.description ? (
                    <span className="block text-xs text-muted-foreground">{item.description}</span>
                  ) : null}
                </Button>
              </li>
            ))}
          </ul>
        ) : null}
      </div>
    </SearchDialog>
  )
}
