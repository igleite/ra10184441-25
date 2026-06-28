ALTER TABLE [tenants].[customer]
    ADD CONSTRAINT [FK_customer_organization]
        FOREIGN KEY ([organization_id]) REFERENCES [tenants].[organization] ([id])
GO
