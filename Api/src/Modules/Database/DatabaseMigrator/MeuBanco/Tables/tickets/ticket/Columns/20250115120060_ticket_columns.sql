CREATE TABLE [tickets].[ticket]
(
    [id]                 UNIQUEIDENTIFIER NOT NULL,
    [created_at]         DATETIME2        NOT NULL,
    [updated_at]         DATETIME2        NOT NULL,
    [row_version]        ROWVERSION       NOT NULL,
    [organization_id]    UNIQUEIDENTIFIER NOT NULL,
    [deleted_at]         DATETIME2        NULL,
    [customer_id]        UNIQUEIDENTIFIER NOT NULL,
    [artifact_id]        UNIQUEIDENTIFIER NULL,
    [status_id]          UNIQUEIDENTIFIER NOT NULL,
    [classification_id]  UNIQUEIDENTIFIER NULL,
    [allocation_center]  INT              NOT NULL
        CONSTRAINT [DF_ticket_allocation_center] DEFAULT 1,
    [created_by_user_id] UNIQUEIDENTIFIER NOT NULL,
    [description]        NVARCHAR(MAX)    NOT NULL,
    [resolution]         NVARCHAR(MAX)    NULL
)
GO
