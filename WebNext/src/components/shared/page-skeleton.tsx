import { Skeleton } from '@/components/ui/skeleton'

export interface PageSkeletonProps {
  fields?: number
}

export function PageSkeleton({ fields = 4 }: PageSkeletonProps) {
  return (
    <div className="mx-auto max-w-2xl space-y-6">
      <div className="space-y-2 border-b pb-6">
        <Skeleton className="h-4 w-32" />
        <Skeleton className="h-8 w-64" />
        <Skeleton className="h-4 w-96" />
      </div>
      {Array.from({ length: fields }).map((_, index) => (
        <div key={index} className="space-y-2">
          <Skeleton className="h-4 w-24" />
          <Skeleton className="h-9 w-full" />
        </div>
      ))}
      <Skeleton className="h-9 w-28" />
    </div>
  )
}
