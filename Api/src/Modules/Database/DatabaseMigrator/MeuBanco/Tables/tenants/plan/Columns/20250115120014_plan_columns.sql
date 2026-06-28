CREATE TABLE [tenants].[plan]
(
    [id]           UNIQUEIDENTIFIER NOT NULL,
    [created_at]   DATETIME2        NOT NULL,
    [updated_at]   DATETIME2        NOT NULL,
    [row_version]  ROWVERSION       NOT NULL,
    [inactived_at] DATETIME2        NULL,
    [name]         NVARCHAR(255)    NOT NULL,
    [description]  NVARCHAR(MAX)    NULL,
    [max_users]    INT              NOT NULL,
    [max_clients]  INT              NOT NULL,
    [max_tickets]  INT              NOT NULL
)
GO
