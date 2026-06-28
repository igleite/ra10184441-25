'use client'

import { useState } from 'react'

import {
  EntitySearchDialog,
  type EntitySearchItem,
} from '@/components/shared/entity-search-dialog'
import { Button } from '@/components/ui/button'
import { Label } from '@/components/ui/label'

interface PlanCatalogDto {
  id: string
  description: string
}

function mapPlanCatalog(raw: Record<string, unknown>): EntitySearchItem | null {
  const plan = raw as unknown as PlanCatalogDto
  if (!plan.id) {
    return null
  }
  return {
    id: plan.id,
    label: plan.description || plan.id,
    description: plan.id,
  }
}

export interface PlanCatalogPickerProps {
  id?: string
  label?: string
  value: string
  displayLabel?: string
  onChange: (planId: string, label: string) => void
  required?: boolean
  error?: string | null
}

export function PlanCatalogPicker({
  id = 'planId',
  label = 'Plano do catálogo',
  value,
  displayLabel,
  onChange,
  required = false,
  error,
}: PlanCatalogPickerProps) {
  const [open, setOpen] = useState(false)
  const [selectedLabel, setSelectedLabel] = useState(displayLabel ?? '')

  return (
    <div className="space-y-2">
      <Label htmlFor={id}>{label}</Label>
      <div className="flex flex-col gap-2 sm:flex-row sm:items-center">
        <Button type="button" variant="outline" onClick={() => setOpen(true)}>
          {value ? 'Trocar plano' : 'Buscar plano'}
        </Button>
        {value ? (
          <p className="text-sm">
            <span className="font-medium">{selectedLabel || value}</span>
          </p>
        ) : (
          <p className="text-sm text-muted-foreground">Nenhum plano selecionado.</p>
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
          title="Selecionar plano"
          description="Busque pelo plano no catálogo global da plataforma."
          endpoint="api/plans?pageIndex=1&pageSize=100"
          mapItem={mapPlanCatalog}
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
