CREATE UNIQUE INDEX [UQ_customer_organization_id_document]
    ON [tenants].[customer] ([organization_id], [document])
    WHERE [inactived_at] IS NULL
GO

CREATE INDEX [IX_customer_organization_id]
    ON [tenants].[customer] ([organization_id])
GO

CREATE INDEX [IX_customer_inactived_at]
    ON [tenants].[customer] ([inactived_at])
GO
