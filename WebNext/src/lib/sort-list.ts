export type SortDirection = 'asc' | 'desc'

export function sortListByField<T extends object>(
  items: T[],
  field: keyof T,
  direction: SortDirection = 'asc',
): T[] {
  const sorted = [...items].sort((left, right) => {
    const leftValue = left[field]
    const rightValue = right[field]

    if (leftValue === rightValue) {
      return 0
    }

    if (leftValue === null || leftValue === undefined) {
      return 1
    }

    if (rightValue === null || rightValue === undefined) {
      return -1
    }

    const comparison =
      typeof leftValue === 'number' && typeof rightValue === 'number'
        ? leftValue - rightValue
        : String(leftValue).localeCompare(String(rightValue), 'pt-BR', {
            sensitivity: 'base',
          })

    return direction === 'asc' ? comparison : -comparison
  })

  return sorted
}
