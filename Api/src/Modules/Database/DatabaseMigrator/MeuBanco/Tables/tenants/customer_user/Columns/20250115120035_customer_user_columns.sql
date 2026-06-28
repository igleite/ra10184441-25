CREATE TABLE [tenants].[customer_user]
(
    [id]              UNIQUEIDENTIFIER NOT NULL,
    [created_at]      DATETIME2        NOT NULL,
    [updated_at]      DATETIME2        NOT NULL,
    [row_version]     ROWVERSION       NOT NULL,
    [organization_id] UNIQUEIDENTIFIER NOT NULL,
    [inactived_at]    DATETIME2        NULL,
    [customer_id]     UNIQUEIDENTIFIER NOT NULL,
    [user_id]         UNIQUEIDENTIFIER NOT NULL,
    [role_id]         UNIQUEIDENTIFIER NOT NULL
)
GO
