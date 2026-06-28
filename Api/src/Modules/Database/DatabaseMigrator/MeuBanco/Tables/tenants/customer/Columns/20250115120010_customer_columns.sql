CREATE TABLE [tenants].[customer]
(
    [id]              UNIQUEIDENTIFIER NOT NULL,
    [created_at]      DATETIME2        NOT NULL,
    [updated_at]      DATETIME2        NOT NULL,
    [row_version]     ROWVERSION       NOT NULL,
    [organization_id] UNIQUEIDENTIFIER NOT NULL,
    [inactived_at]    DATETIME2        NULL,
    [name]            NVARCHAR(255)    NOT NULL,
    [document]        NVARCHAR(50)     NOT NULL
)
GO
