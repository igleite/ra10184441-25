ALTER TABLE [tenants].[artifact_type]
    ADD CONSTRAINT [FK_artifact_type_organization]
        FOREIGN KEY ([organization_id]) REFERENCES [tenants].[organization] ([id])
GO
