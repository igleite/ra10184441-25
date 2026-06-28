CREATE UNIQUE INDEX [UQ_customer_user_organization_id_customer_id_user_id]
    ON [tenants].[customer_user] ([organization_id], [customer_id], [user_id])
    WHERE [inactived_at] IS NULL
GO

CREATE INDEX [IX_customer_user_organization_id]
    ON [tenants].[customer_user] ([organization_id])
GO

CREATE INDEX [IX_customer_user_customer_id]
    ON [tenants].[customer_user] ([customer_id])
GO

CREATE INDEX [IX_customer_user_user_id]
    ON [tenants].[customer_user] ([user_id])
GO

CREATE INDEX [IX_customer_user_role_id]
    ON [tenants].[customer_user] ([role_id])
GO

CREATE INDEX [IX_customer_user_inactived_at]
    ON [tenants].[customer_user] ([inactived_at])
GO