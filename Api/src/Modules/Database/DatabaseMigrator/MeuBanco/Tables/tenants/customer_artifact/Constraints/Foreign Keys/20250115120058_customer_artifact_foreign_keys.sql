ALTER TABLE [tenants].[customer_artifact]
    ADD CONSTRAINT [FK_customer_artifact_customer]
        FOREIGN KEY ([customer_id]) REFERENCES [tenants].[customer] ([id])
GO

ALTER TABLE [tenants].[customer_artifact]
    ADD CONSTRAINT [FK_customer_artifact_artifact]
        FOREIGN KEY ([artifact_id]) REFERENCES [tenants].[artifact] ([id])
GO
