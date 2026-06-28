CREATE TABLE [tenants].[artifact]
(
    [id]               UNIQUEIDENTIFIER NOT NULL,
    [created_at]       DATETIME2        NOT NULL,
    [updated_at]       DATETIME2        NOT NULL,
    [row_version]      ROWVERSION       NOT NULL,
    [inactived_at]     DATETIME2        NULL,
    [artifact_type_id] UNIQUEIDENTIFIER NOT NULL,
    [name]             NVARCHAR(255)    NOT NULL,
    [code]             NVARCHAR(50)     NOT NULL
)
GO
