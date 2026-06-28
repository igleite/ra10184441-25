ALTER TABLE [tenants].[team]
    ADD CONSTRAINT [FK_team_organization]
        FOREIGN KEY ([organization_id]) REFERENCES [tenants].[organization] ([id])
GO

ALTER TABLE [tenants].[team]
    ADD CONSTRAINT [FK_team_role]
        FOREIGN KEY ([role_id]) REFERENCES [tenants].[role] ([id])
GO
