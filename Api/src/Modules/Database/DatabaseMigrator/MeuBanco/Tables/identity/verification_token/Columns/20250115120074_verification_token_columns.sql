CREATE TABLE [identity].[verification_token]
(
    [id]          UNIQUEIDENTIFIER NOT NULL,
    [created_at]  DATETIME2        NOT NULL,
    [updated_at]  DATETIME2        NOT NULL,
    [row_version] ROWVERSION       NOT NULL,
    [token]       NVARCHAR(512)    NOT NULL,
    [email]       NVARCHAR(255)    NOT NULL,
    [expires_at]  DATETIME2        NOT NULL
)
GO
