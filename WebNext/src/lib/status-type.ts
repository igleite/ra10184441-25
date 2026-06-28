export interface StatusTypeObject {
  value: number
}

export type StatusTypeField = number | StatusTypeObject

export const STATUS_TYPE_OPTIONS = [
  { value: '1', label: 'Aberto' },
  { value: '2', label: 'Fechado' },
] as const

export function parseStatusTypeValue(type: StatusTypeField): number {
  if (typeof type === 'number') {
    return type
  }
  return type.value
}

export function statusTypeLabel(type: StatusTypeField): string {
  const value = parseStatusTypeValue(type)
  if (value === 1) {
    return 'Aberto'
  }
  if (value === 2) {
    return 'Fechado'
  }
  return String(value)
}

export function statusTypeToFormValue(type: StatusTypeField): string {
  return String(parseStatusTypeValue(type))
}
