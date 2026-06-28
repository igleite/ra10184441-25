CREATE TABLE [tenants].[role]
(
    [id]         UNIQUEIDENTIFIER NOT NULL,
    [name]       NVARCHAR(255)    NOT NULL,
    [scope]      NVARCHAR(50)     NOT NULL,
    [created_at] DATETIME2        NOT NULL,
    [updated_at] DATETIME2        NOT NULL
)
GO
