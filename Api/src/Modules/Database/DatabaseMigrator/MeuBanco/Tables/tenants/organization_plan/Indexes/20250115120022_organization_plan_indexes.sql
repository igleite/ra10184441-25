CREATE INDEX [IX_organization_plan_organization_id]
    ON [tenants].[organization_plan] ([organization_id])
GO

CREATE INDEX [IX_organization_plan_plan_id]
    ON [tenants].[organization_plan] ([plan_id])
GO

CREATE INDEX [IX_organization_plan_inactived_at]
    ON [tenants].[organization_plan] ([inactived_at])
GO
