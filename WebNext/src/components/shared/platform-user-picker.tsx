'use client'

import { useState } from 'react'

import { EntitySearchDialog, type EntitySearchItem } from '@/components/shared/entity-search-dialog'
import { Button } from '@/components/ui/button'
import { Label } from '@/components/ui/label'

interface PlatformUserDto {
  id: string
  name: string
  email: string
}

function mapPlatformUser(raw: Record<string, unknown>): EntitySearchItem | null {
  const user = raw as unknown as PlatformUserDto
  if (!user.id || !user.email) {
    return null
  }
  return {
    id: user.id,
    label: user.name || user.email,
    description: user.email,
  }
}

export interface PlatformUserPickerProps {
  id?: string
  label?: string
  value: string
  displayLabel?: string
  onChange: (userId: string, label: string) => void
  required?: boolean
  error?: string | null
}

export function PlatformUserPicker({
  id = 'userId',
  label = 'Usuário da plataforma',
  value,
  displayLabel,
  onChange,
  required = false,
  error,
}: PlatformUserPickerProps) {
  const [open, setOpen] = useState(false)
  const [selectedLabel, setSelectedLabel] = useState(displayLabel ?? '')

  return (
    <div className="space-y-2">
      <Label htmlFor={id}>{label}</Label>
      <div className="flex flex-col gap-2 sm:flex-row sm:items-center">
        <Button type="button" variant="outline" onClick={() => setOpen(true)}>
          {value ? 'Trocar usuário' : 'Buscar usuário'}
        </Button>
        {value ? (
          <p className="text-sm">
            <span className="font-medium">{selectedLabel || value}</span>
            <span className="ml-2 text-xs text-muted-foreground">{value}</span>
          </p>
        ) : (
          <p className="text-sm text-muted-foreground">Nenhum usuário selecionado.</p>
        )}
      </div>
      <input type="hidden" id={id} name={id} value={value} required={required} />
      {error ? (
        <p className="text-xs text-destructive" role="alert">
          {error}
        </p>
      ) : null}
      {open ? (
        <EntitySearchDialog
          open={open}
          onOpenChange={setOpen}
          title="Selecionar usuário"
          description="Busque por nome ou e-mail da conta global."
          endpoint="api/users?pageIndex=1&pageSize=100"
          mapItem={mapPlatformUser}
          searchFields={['label', 'description', 'id']}
          onSelect={(item) => {
            onChange(item.id, item.label)
            setSelectedLabel(item.label)
          }}
        />
      ) : null}
    </div>
  )
}
