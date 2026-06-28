ALTER TABLE [tickets].[ticket]
    ADD CONSTRAINT [FK_ticket_organization]
        FOREIGN KEY ([organization_id]) REFERENCES [tenants].[organization] ([id])
GO

ALTER TABLE [tickets].[ticket]
    ADD CONSTRAINT [FK_ticket_customer]
        FOREIGN KEY ([customer_id]) REFERENCES [tenants].[customer] ([id])
GO

ALTER TABLE [tickets].[ticket]
    ADD CONSTRAINT [FK_ticket_artifact]
        FOREIGN KEY ([artifact_id]) REFERENCES [tenants].[artifact] ([id])
GO

ALTER TABLE [tickets].[ticket]
    ADD CONSTRAINT [FK_ticket_status_reason]
        FOREIGN KEY ([status_id]) REFERENCES [tickets].[status_reason] ([id])
GO

ALTER TABLE [tickets].[ticket]
    ADD CONSTRAINT [FK_ticket_ticket_classification]
        FOREIGN KEY ([classification_id]) REFERENCES [tickets].[ticket_classification] ([id])
GO

ALTER TABLE [tickets].[ticket]
    ADD CONSTRAINT [FK_ticket_user_created_by]
        FOREIGN KEY ([created_by_user_id]) REFERENCES [tenants].[user] ([id])
GO
