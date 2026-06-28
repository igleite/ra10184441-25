'use client'

import { useEffect, useMemo, useState } from 'react'

import { apiGet } from '@/lib/api/client'
import type { PageDto } from '@/lib/api/types'

interface PlatformUserDto {
  id: string
  name: string
  email: string
}

const LOOKUP_PAGE_SIZE = 100

export function usePlatformUserLabels() {
  const [labels, setLabels] = useState<Map<string, string>>(new Map())
  const [loaded, setLoaded] = useState(false)

  useEffect(() => {
    let cancelled = false

    void apiGet<PageDto<PlatformUserDto>>(`api/users?pageIndex=1&pageSize=${LOOKUP_PAGE_SIZE}`)
      .then((page) => {
        if (cancelled) {
          return
        }

        setLabels(
          new Map(
            (page.items ?? []).map((user) => [
              user.id,
              user.name?.trim() ? user.name : user.email,
            ]),
          ),
        )
        setLoaded(true)
      })
      .catch(() => {
        if (!cancelled) {
          setLabels(new Map())
          setLoaded(true)
        }
      })

    return () => {
      cancelled = true
    }
  }, [])

  const userName = useMemo(
    () => (id: string) => labels.get(id) ?? id,
    [labels],
  )

  return { labels, loaded, userName }
}
