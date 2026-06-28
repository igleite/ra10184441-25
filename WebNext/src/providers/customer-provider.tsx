'use client'

import {
  createContext,
  useContext,
  useMemo,
  type ReactNode,
} from 'react'

import { useAuth } from '@/providers/auth-provider'
import { ROLES } from '@/types/auth'

interface CustomerContextValue {
  customerId: string | null
  isClientUser: boolean
}

const CustomerContext = createContext<CustomerContextValue | null>(null)

export function CustomerProvider({ children }: { children: ReactNode }) {
  const { user } = useAuth()

  const value = useMemo<CustomerContextValue>(() => {
    const isClientUser =
      user?.roles.includes(ROLES.clientAdmin) ||
      user?.roles.includes(ROLES.clientMember) ||
      false

    return {
      customerId: user?.customerId ?? null,
      isClientUser,
    }
  }, [user])

  return (
    <CustomerContext.Provider value={value}>{children}</CustomerContext.Provider>
  )
}

export function useCustomer(): CustomerContextValue {
  const context = useContext(CustomerContext)
  if (!context) {
    throw new Error('useCustomer deve ser usado dentro de CustomerProvider')
  }
  return context
}
