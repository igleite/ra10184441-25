CREATE UNIQUE INDEX [UQ_customer_artifact_customer_id_artifact_id]
    ON [tenants].[customer_artifact] ([customer_id], [artifact_id])
    WHERE [inactived_at] IS NULL
GO

CREATE INDEX [IX_customer_artifact_customer_id]
    ON [tenants].[customer_artifact] ([customer_id])
GO

CREATE INDEX [IX_customer_artifact_artifact_id]
    ON [tenants].[customer_artifact] ([artifact_id])
GO

CREATE INDEX [IX_customer_artifact_inactived_at]
    ON [tenants].[customer_artifact] ([inactived_at])
GO
