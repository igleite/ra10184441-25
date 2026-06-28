CREATE TABLE [tickets].[chat]
(
    [id]          UNIQUEIDENTIFIER NOT NULL,
    [created_at]  DATETIME2        NOT NULL,
    [updated_at]  DATETIME2        NOT NULL,
    [row_version] ROWVERSION       NOT NULL,
    [deleted_at]  DATETIME2        NULL,
    [ticket_id]   UNIQUEIDENTIFIER NOT NULL,
    [user_id]     UNIQUEIDENTIFIER NOT NULL,
    [message]     NVARCHAR(MAX)    NOT NULL
)
GO
