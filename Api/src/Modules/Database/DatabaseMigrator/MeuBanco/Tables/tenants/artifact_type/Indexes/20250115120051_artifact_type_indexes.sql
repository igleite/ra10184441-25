CREATE UNIQUE INDEX [UQ_artifact_type_organization_id_name]
    ON [tenants].[artifact_type] ([organization_id], [name])
    WHERE [inactived_at] IS NULL
GO

CREATE INDEX [IX_artifact_type_organization_id]
    ON [tenants].[artifact_type] ([organization_id])
GO

CREATE INDEX [IX_artifact_type_inactived_at]
    ON [tenants].[artifact_type] ([inactived_at])
GO
