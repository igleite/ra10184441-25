CREATE UNIQUE INDEX [UQ_organization_user_organization_id_user_id] ON [tenants].[organization_user] ([organization_id], [user_id]) WHERE [inactived_at] IS NULL
GO

CREATE INDEX [IX_organization_user_organization_id] ON [tenants].[organization_user] ([organization_id])
GO

CREATE INDEX [IX_organization_user_user_id] ON [tenants].[organization_user] ([user_id])
GO

CREATE INDEX [IX_organization_user_team_id] ON [tenants].[organization_user] ([team_id])
GO

CREATE INDEX [IX_organization_user_inactived_at] ON [tenants].[organization_user] ([inactived_at])
GO