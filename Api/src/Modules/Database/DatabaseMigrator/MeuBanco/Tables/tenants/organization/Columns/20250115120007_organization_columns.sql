CREATE TABLE [tenants].[organization]
(
    [id]          UNIQUEIDENTIFIER NOT NULL,
    [created_at]  DATETIME2        NOT NULL,
    [updated_at]  DATETIME2        NOT NULL,
    [row_version] ROWVERSION       NOT NULL,
    [name]        NVARCHAR(255)    NOT NULL,
    [document]    NVARCHAR(50)     NOT NULL,
    [slug]        NVARCHAR(50)     NOT NULL
)
GO
