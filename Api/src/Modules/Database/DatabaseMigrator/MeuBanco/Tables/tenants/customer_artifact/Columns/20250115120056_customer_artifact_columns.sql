CREATE TABLE [tenants].[customer_artifact]
(
    [id]           UNIQUEIDENTIFIER NOT NULL,
    [created_at]   DATETIME2        NOT NULL,
    [updated_at]   DATETIME2        NOT NULL,
    [row_version]  ROWVERSION       NOT NULL,
    [inactived_at] DATETIME2        NULL,
    [customer_id]  UNIQUEIDENTIFIER NOT NULL,
    [artifact_id]  UNIQUEIDENTIFIER NOT NULL
)
GO
