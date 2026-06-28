ALTER TABLE [tickets].[status_reason]
    ADD CONSTRAINT [FK_status_reason_organization]
        FOREIGN KEY ([organization_id]) REFERENCES [tenants].[organization] ([id])
GO
