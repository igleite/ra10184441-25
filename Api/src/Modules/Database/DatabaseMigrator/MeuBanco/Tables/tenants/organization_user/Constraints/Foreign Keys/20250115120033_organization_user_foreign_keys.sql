ALTER TABLE [tenants].[organization_user]
    ADD CONSTRAINT [FK_organization_user_organization]
        FOREIGN KEY ([organization_id]) REFERENCES [tenants].[organization] ([id])
GO

ALTER TABLE [tenants].[organization_user]
    ADD CONSTRAINT [FK_organization_user_user]
        FOREIGN KEY ([user_id]) REFERENCES [tenants].[user] ([id])
GO

ALTER TABLE [tenants].[organization_user]
    ADD CONSTRAINT [FK_organization_user_team]
        FOREIGN KEY ([team_id]) REFERENCES [tenants].[team] ([id])
GO
