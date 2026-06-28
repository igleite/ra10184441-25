CREATE UNIQUE INDEX [UQ_organization_document]
    ON [tenants].[organization] ([document])
GO

CREATE UNIQUE INDEX [UQ_organization_slug]
    ON [tenants].[organization] ([slug])
GO
