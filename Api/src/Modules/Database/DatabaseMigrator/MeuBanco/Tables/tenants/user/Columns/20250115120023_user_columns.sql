CREATE TABLE [tenants].[user]
(
    [id]           UNIQUEIDENTIFIER NOT NULL,
    [created_at]   DATETIME2        NOT NULL,
    [updated_at]   DATETIME2        NOT NULL,
    [row_version]  ROWVERSION       NOT NULL,
    [inactived_at] DATETIME2        NULL,
    [name]         NVARCHAR(255)    NOT NULL,
    [email]        NVARCHAR(255)    NOT NULL,
    [role_id]      UNIQUEIDENTIFIER NULL
)
GO
