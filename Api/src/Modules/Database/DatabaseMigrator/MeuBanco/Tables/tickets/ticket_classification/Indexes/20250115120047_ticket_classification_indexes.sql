CREATE UNIQUE INDEX [UQ_ticket_classification_organization_id_code]
    ON [tickets].[ticket_classification] ([organization_id], [code])
    WHERE [inactived_at] IS NULL
GO

CREATE UNIQUE INDEX [UQ_ticket_classification_organization_id_name]
    ON [tickets].[ticket_classification] ([organization_id], [name])
    WHERE [inactived_at] IS NULL
GO

CREATE INDEX [IX_ticket_classification_organization_id]
    ON [tickets].[ticket_classification] ([organization_id])
GO

CREATE INDEX [IX_ticket_classification_inactived_at]
    ON [tickets].[ticket_classification] ([inactived_at])
GO
