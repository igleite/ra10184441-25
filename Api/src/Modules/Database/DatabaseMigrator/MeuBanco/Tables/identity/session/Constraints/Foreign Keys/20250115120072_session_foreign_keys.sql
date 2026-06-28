ALTER TABLE [identity].[session]
    ADD CONSTRAINT [FK_session_user]
        FOREIGN KEY ([user_id]) REFERENCES [tenants].[user] ([id])
        ON DELETE CASCADE
GO
