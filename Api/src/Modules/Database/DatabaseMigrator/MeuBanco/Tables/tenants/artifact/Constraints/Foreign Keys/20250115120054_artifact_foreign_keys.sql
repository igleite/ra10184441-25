ALTER TABLE [tenants].[artifact]
    ADD CONSTRAINT [FK_artifact_artifact_type]
        FOREIGN KEY ([artifact_type_id]) REFERENCES [tenants].[artifact_type] ([id])
GO
