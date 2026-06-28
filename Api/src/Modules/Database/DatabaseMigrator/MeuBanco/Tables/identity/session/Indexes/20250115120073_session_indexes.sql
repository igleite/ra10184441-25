CREATE UNIQUE INDEX [UQ_session_token]
    ON [identity].[session] ([token])
GO

CREATE INDEX [IX_session_user_id]
    ON [identity].[session] ([user_id])
GO
