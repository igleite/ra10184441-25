CREATE TABLE [tickets].[status_reason]
(
    [id]                 UNIQUEIDENTIFIER NOT NULL,
    [created_at]         DATETIME2        NOT NULL,
    [updated_at]         DATETIME2        NOT NULL,
    [row_version]        ROWVERSION       NOT NULL,
    [organization_id]    UNIQUEIDENTIFIER NOT NULL,
    [inactived_at]       DATETIME2        NULL,
    [type]               INT              NOT NULL,
    [name]               NVARCHAR(255)    NOT NULL,
    [is_opening_default] BIT              NOT NULL
)
GO
