const dashboardPaths = new Set(['/admin', '/account', '/app', '/portal'])

export function isNavItemActive(pathname: string, href: string): boolean {
  if (dashboardPaths.has(href)) {
    return pathname === href
  }

  return pathname === href || pathname.startsWith(`${href}/`)
}
