CREATE UNIQUE INDEX [UQ_artifact_artifact_type_id_name]
    ON [tenants].[artifact] ([artifact_type_id], [name])
    WHERE [inactived_at] IS NULL
GO

CREATE UNIQUE INDEX [UQ_artifact_artifact_type_id_code]
    ON [tenants].[artifact] ([artifact_type_id], [code])
    WHERE [inactived_at] IS NULL
GO

CREATE INDEX [IX_artifact_artifact_type_id]
    ON [tenants].[artifact] ([artifact_type_id])
GO

CREATE INDEX [IX_artifact_inactived_at]
    ON [tenants].[artifact] ([inactived_at])
GO
