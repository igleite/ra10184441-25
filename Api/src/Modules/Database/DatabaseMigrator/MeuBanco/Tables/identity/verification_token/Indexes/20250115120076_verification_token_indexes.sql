CREATE UNIQUE INDEX [UQ_verification_token_token]
    ON [identity].[verification_token] ([token])
GO

CREATE INDEX [IX_verification_token_email]
    ON [identity].[verification_token] ([email])
GO
