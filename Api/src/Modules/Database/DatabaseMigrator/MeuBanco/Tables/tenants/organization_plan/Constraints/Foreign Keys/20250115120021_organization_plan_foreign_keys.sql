ALTER TABLE [tenants].[organization_plan]
    ADD CONSTRAINT [FK_organization_plan_organization]
        FOREIGN KEY ([organization_id]) REFERENCES [tenants].[organization] ([id])
GO

ALTER TABLE [tenants].[organization_plan]
    ADD CONSTRAINT [FK_organization_plan_plan]
        FOREIGN KEY ([plan_id]) REFERENCES [tenants].[plan] ([id])
GO
