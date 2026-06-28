CREATE UNIQUE INDEX [UQ_status_reason_organization_id_name] ON [tickets].[status_reason] ([organization_id], [name]) WHERE [inactived_at] IS NULL
GO

CREATE INDEX [IX_status_reason_organization_id] ON [tickets].[status_reason] ([organization_id])
GO

CREATE INDEX [IX_status_reason_inactived_at] ON [tickets].[status_reason] ([inactived_at])
GO
