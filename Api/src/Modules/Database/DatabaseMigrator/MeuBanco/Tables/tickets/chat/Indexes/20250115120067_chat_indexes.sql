CREATE INDEX [IX_chat_ticket_id] ON [tickets].[chat] ([ticket_id])
GO

CREATE INDEX [IX_chat_user_id] ON [tickets].[chat] ([user_id])
GO

CREATE INDEX [IX_chat_deleted_at] ON [tickets].[chat] ([deleted_at])
GO
