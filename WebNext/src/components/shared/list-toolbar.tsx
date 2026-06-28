'use client'

import type { ReactNode } from 'react'

import { SelectField } from '@/components/shared/select-field'
import { Input } from '@/components/ui/input'
import { cn } from '@/lib/utils'
import type { SortDirection } from '@/lib/sort-list'

export interface ListSortOption {
  value: string
  label: string
}

export interface ListToolbarProps {
  searchValue: string
  onSearchChange: (value: string) => void
  searchPlaceholder?: string
  sortField?: string
  onSortFieldChange?: (value: string) => void
  sortDirection?: SortDirection
  onSortDirectionChange?: (value: SortDirection) => void
  sortOptions?: ListSortOption[]
  actions?: ReactNode
  className?: string
}

const DIRECTION_OPTIONS = [
  { value: 'asc', label: 'Crescente' },
  { value: 'desc', label: 'Decrescente' },
] as const

export function ListToolbar({
  searchValue,
  onSearchChange,
  searchPlaceholder = 'Buscar na lista...',
  sortField,
  onSortFieldChange,
  sortDirection = 'asc',
  onSortDirectionChange,
  sortOptions,
  actions,
  className,
}: ListToolbarProps) {
  const showSort = Boolean(sortOptions?.length && onSortFieldChange && onSortDirectionChange)

  return (
    <div className={cn('flex flex-col gap-3', className)}>
      <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
        <Input
          type="search"
          value={searchValue}
          onChange={(event) => onSearchChange(event.target.value)}
          placeholder={searchPlaceholder}
          className="max-w-md"
          aria-label="Buscar"
        />
        {actions ? <div className="flex flex-wrap gap-2">{actions}</div> : null}
      </div>

      {showSort ? (
        <div className="grid gap-3 sm:grid-cols-2 lg:max-w-xl">
          <SelectField
            id="list-sort-field"
            label="Ordenar por"
            value={sortField ?? ''}
            onChange={onSortFieldChange!}
            options={sortOptions!}
            placeholder="Campo"
          />
          <SelectField
            id="list-sort-direction"
            label="Direção"
            value={sortDirection}
            onChange={(value) => onSortDirectionChange!(value as SortDirection)}
            options={[...DIRECTION_OPTIONS]}
          />
        </div>
      ) : null}
    </div>
  )
}
