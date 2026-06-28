ALTER TABLE [tenants].[customer_user]
    ADD CONSTRAINT [FK_customer_user_organization]
        FOREIGN KEY ([organization_id]) REFERENCES [tenants].[organization] ([id])
GO

ALTER TABLE [tenants].[customer_user]
    ADD CONSTRAINT [FK_customer_user_customer]
        FOREIGN KEY ([customer_id]) REFERENCES [tenants].[customer] ([id])
GO

ALTER TABLE [tenants].[customer_user]
    ADD CONSTRAINT [FK_customer_user_user]
        FOREIGN KEY ([user_id]) REFERENCES [tenants].[user] ([id])
GO

ALTER TABLE [tenants].[customer_user]
    ADD CONSTRAINT [FK_customer_user_role]
        FOREIGN KEY ([role_id]) REFERENCES [tenants].[role] ([id])
GO
