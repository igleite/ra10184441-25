CREATE UNIQUE INDEX [UQ_user_email]
    ON [tenants].[user] ([email])
    WHERE [inactived_at] IS NULL
GO

CREATE INDEX [IX_user_role_id] ON [tenants].[user] ([role_id])
GO

CREATE INDEX [IX_user_inactived_at] ON [tenants].[user] ([inactived_at])
GO