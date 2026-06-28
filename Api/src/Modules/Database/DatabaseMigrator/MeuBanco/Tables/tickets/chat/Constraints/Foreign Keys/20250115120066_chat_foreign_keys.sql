ALTER TABLE [tickets].[chat]
    ADD CONSTRAINT [FK_chat_ticket]
        FOREIGN KEY ([ticket_id]) REFERENCES [tickets].[ticket] ([id])
GO

ALTER TABLE [tickets].[chat]
    ADD CONSTRAINT [FK_chat_user]
        FOREIGN KEY ([user_id]) REFERENCES [tenants].[user] ([id])
GO
