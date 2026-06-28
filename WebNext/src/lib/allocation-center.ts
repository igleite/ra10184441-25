export const ALLOCATION_CENTER_OPTIONS = [
  { value: '1', label: 'Cliente' },
  { value: '2', label: 'Organização' },
] as const

export function allocationCenterLabel(value: number): string {
  if (value === 1) {
    return 'Cliente'
  }
  if (value === 2) {
    return 'Organização'
  }
  return String(value)
}
