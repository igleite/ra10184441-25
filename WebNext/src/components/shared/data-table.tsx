'use client'

import type { ReactNode } from 'react'

import { Skeleton } from '@/components/ui/skeleton'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'

export interface ColumnDef<T> {
  id: string
  header: string
  cell: (row: T) => ReactNode
  className?: string
}

export interface DataTablePagination {
  pageIndex: number
  pageSize: number
  totalItemCount: number
  onPageChange: (pageIndex: number) => void
}

export interface DataTableProps<T> {
  columns: ColumnDef<T>[]
  data: T[]
  isLoading?: boolean
  emptyMessage?: string
  onRowClick?: (row: T) => void
  pagination?: DataTablePagination
}

export function DataTable<T>({
  columns,
  data,
  isLoading = false,
  emptyMessage = 'Nenhum registro encontrado.',
  onRowClick,
  pagination,
}: DataTableProps<T>) {
  const totalPages = pagination
    ? Math.max(1, Math.ceil(pagination.totalItemCount / pagination.pageSize))
    : 1

  return (
    <div className="space-y-4">
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              {columns.map((column) => (
                <TableHead key={column.id} className={column.className}>
                  {column.header}
                </TableHead>
              ))}
            </TableRow>
          </TableHeader>
          <TableBody>
            {isLoading
              ? Array.from({ length: 5 }).map((_, rowIndex) => (
                  <TableRow key={`skeleton-${rowIndex}`}>
                    {columns.map((column) => (
                      <TableCell key={column.id}>
                        <Skeleton className="h-4 w-full" />
                      </TableCell>
                    ))}
                  </TableRow>
                ))
              : null}

            {!isLoading && data.length === 0 ? (
              <TableRow>
                <TableCell
                  colSpan={columns.length}
                  className="h-24 text-center text-muted-foreground"
                >
                  {emptyMessage}
                </TableCell>
              </TableRow>
            ) : null}

            {!isLoading
              ? data.map((row, rowIndex) => (
                  <TableRow
                    key={rowIndex}
                    className={onRowClick ? 'cursor-pointer' : undefined}
                    onClick={onRowClick ? () => onRowClick(row) : undefined}
                  >
                    {columns.map((column) => (
                      <TableCell key={column.id} className={column.className}>
                        {column.cell(row)}
                      </TableCell>
                    ))}
                  </TableRow>
                ))
              : null}
          </TableBody>
        </Table>
      </div>

      {pagination ? (
        <div className="flex items-center justify-between text-xs text-muted-foreground">
          <span>
            Página {pagination.pageIndex + 1} de {totalPages}
          </span>
          <div className="flex gap-2">
            <button
              type="button"
              className="rounded border px-2 py-1 disabled:opacity-50"
              disabled={pagination.pageIndex <= 0}
              onClick={() => pagination.onPageChange(pagination.pageIndex - 1)}
            >
              Anterior
            </button>
            <button
              type="button"
              className="rounded border px-2 py-1 disabled:opacity-50"
              disabled={pagination.pageIndex + 1 >= totalPages}
              onClick={() => pagination.onPageChange(pagination.pageIndex + 1)}
            >
              Próxima
            </button>
          </div>
        </div>
      ) : null}
    </div>
  )
}
