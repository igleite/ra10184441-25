export function filterListByQuery<T extends object>(
  items: T[],
  query: string,
  fields: (keyof T)[],
): T[] {
  const normalized = query.trim().toLowerCase()
  if (!normalized) {
    return items
  }

  return items.filter((item) =>
    fields.some((field) => {
      const value = item[field]
      if (value === null || value === undefined) {
        return false
      }
      return String(value).toLowerCase().includes(normalized)
    }),
  )
}
