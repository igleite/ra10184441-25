CREATE TABLE [identity].[session]
(
    [id]           UNIQUEIDENTIFIER NOT NULL,
    [created_at]   DATETIME2        NOT NULL,
    [updated_at]   DATETIME2        NOT NULL,
    [row_version]  ROWVERSION       NOT NULL,
    [user_id]      UNIQUEIDENTIFIER NOT NULL,
    [token]        NVARCHAR(2048)   NOT NULL,
    [expires_at]   DATETIME2        NOT NULL,
    [last_used_at] DATETIME2        NOT NULL
)
GO
