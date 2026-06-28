ALTER TABLE [tenants].[user]
    ADD CONSTRAINT [FK_user_role]
        FOREIGN KEY ([role_id]) REFERENCES [tenants].[role] ([id])
GO
