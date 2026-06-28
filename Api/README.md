# Service Desk — Modular Monolith

Protótipo/demonstração de um service desk corporativo em **.NET 8**, com **DDD**, **Clean Architecture**, **CQRS** e arquitetura **multi-tenant**. Ainda **não substitui** a solução em produção e **não atingiu MVP**.

## Stack

- ASP.NET Core 8, MediatR, FluentValidation, Dapper
- SQL Server — schema via `DatabaseMigrator` (sem EF Core)
- Autenticação inicial: **Magic Link por e-mail**

## Estrutura

```
src/
├── Api/                 # Host HTTP — controla o acesso aos módulos
├── BuildingBlocks/      # Componentes compartilhados (Domain, Application, Infrastructure)
└── Modules/
    ├── Database/        # DatabaseMigrator — DDL, índices, seeds
    ├── FeatureFlags/    # Gerenciamento de features
    ├── Identity/        # Autenticação
    ├── Tenant/          # Organizações (tenants)
    ├── Tickets/         # Tickets
    └── IA/              # Placeholder (sem implementação definida)
```

## Módulos

| Módulo | Status |
|--------|--------|
| Database | Ativo |
| FeatureFlags | Ativo |
| Identity | Ativo (Magic Link) |
| Tenant | Ativo |
| Tickets | Ativo |
| IA | Criado, sem implementação |
| Billing | Previsão futura |

## Princípios

- Bounded contexts isolados; comunicação entre módulos por eventos de domínio
- CQRS: commands (escrita) e queries (leitura com DTOs)
- Persistência exclusivamente com **Dapper**; evolução do banco via **DatabaseMigrator**
- Isolamento multi-tenant por `OrganizationId`
- Autorização: todas as operações permitidas neste estágio (será implementada depois)

## Como executar

```bash
dotnet restore
dotnet build
dotnet run --project src/Api/Api.csproj
```

Swagger (Development): `https://localhost:5001/swagger`

Connection string: `SdpNewConnection` em `src/Api/appsettings.json`.
