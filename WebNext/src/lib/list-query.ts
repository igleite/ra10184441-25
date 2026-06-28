import { filterListByQuery } from '@/lib/filter-list'
import { sortListByField, type SortDirection } from '@/lib/sort-list'

export function queryList<T extends object>(
  items: T[],
  options: {
    search?: string
    searchFields?: (keyof T)[]
    sortField?: keyof T
    sortDirection?: SortDirection
  },
): T[] {
  const { search = '', searchFields = [], sortField, sortDirection = 'asc' } = options

  const filtered =
    searchFields.length > 0 ? filterListByQuery(items, search, searchFields) : items

  if (!sortField) {
    return filtered
  }

  return sortListByField(filtered, sortField, sortDirection)
}
