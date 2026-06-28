ALTER TABLE [tenants].[organization_plan]
    ADD CONSTRAINT [DF_organization_plan_max_users] DEFAULT 0 FOR [max_users]
GO

ALTER TABLE [tenants].[organization_plan]
    ADD CONSTRAINT [DF_organization_plan_max_clients] DEFAULT 0 FOR [max_clients]
GO

ALTER TABLE [tenants].[organization_plan]
    ADD CONSTRAINT [DF_organization_plan_max_tickets] DEFAULT 0 FOR [max_tickets]
GO