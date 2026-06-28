CREATE UNIQUE INDEX [UQ_team_organization_id_code] ON [tenants].[team] ([organization_id], [code]) WHERE [inactived_at] IS NULL
GO

CREATE UNIQUE INDEX [UQ_team_organization_id_name] ON [tenants].[team] ([organization_id], [name]) WHERE [inactived_at] IS NULL
GO

CREATE INDEX [IX_team_organization_id] ON [tenants].[team] ([organization_id])
GO

CREATE INDEX [IX_team_role_id] ON [tenants].[team] ([role_id])
GO

CREATE INDEX [IX_team_inactived_at] ON [tenants].[team] ([inactived_at])
GO