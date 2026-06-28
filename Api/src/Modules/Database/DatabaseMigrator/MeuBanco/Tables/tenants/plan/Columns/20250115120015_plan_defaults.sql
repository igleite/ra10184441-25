ALTER TABLE [tenants].[plan]
    ADD CONSTRAINT [DF_plan_max_users] DEFAULT 0 FOR [max_users]
GO

ALTER TABLE [tenants].[plan]
    ADD CONSTRAINT [DF_plan_max_clients] DEFAULT 0 FOR [max_clients]
GO

ALTER TABLE [tenants].[plan]
    ADD CONSTRAINT [DF_plan_max_tickets] DEFAULT 0 FOR [max_tickets]
GO
