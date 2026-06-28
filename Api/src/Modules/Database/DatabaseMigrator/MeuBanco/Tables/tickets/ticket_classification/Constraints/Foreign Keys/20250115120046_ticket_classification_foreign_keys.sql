ALTER TABLE [tickets].[ticket_classification]
    ADD CONSTRAINT [FK_ticket_classification_organization]
        FOREIGN KEY ([organization_id]) REFERENCES [tenants].[organization] ([id])
GO
