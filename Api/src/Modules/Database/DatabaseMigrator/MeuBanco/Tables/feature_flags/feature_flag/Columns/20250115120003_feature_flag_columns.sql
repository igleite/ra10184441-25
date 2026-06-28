CREATE TABLE [feature_flags].[feature_flag]
(
    [id]          UNIQUEIDENTIFIER NOT NULL,
    [created_at]  DATETIME2        NOT NULL,
    [updated_at]  DATETIME2        NOT NULL,
    [row_version] ROWVERSION       NOT NULL,
    [name]        NVARCHAR(255)    NOT NULL,
    [description] NVARCHAR(MAX)    NOT NULL,
    [value]       BIT              NOT NULL
)
GO
