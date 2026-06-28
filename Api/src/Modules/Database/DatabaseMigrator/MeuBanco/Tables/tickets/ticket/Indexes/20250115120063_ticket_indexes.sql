CREATE INDEX [IX_ticket_organization_id] ON [tickets].[ticket] ([organization_id])
GO

CREATE INDEX [IX_ticket_deleted_at] ON [tickets].[ticket] ([deleted_at])
GO

CREATE INDEX [IX_ticket_customer_id] ON [tickets].[ticket] ([customer_id])
GO

CREATE INDEX [IX_ticket_artifact_id] ON [tickets].[ticket] ([artifact_id])
GO

CREATE INDEX [IX_ticket_status_id] ON [tickets].[ticket] ([status_id])
GO

CREATE INDEX [IX_ticket_classification_id] ON [tickets].[ticket] ([classification_id])
GO

CREATE INDEX [IX_ticket_allocation_center] ON [tickets].[ticket] ([allocation_center])
GO

CREATE INDEX [IX_ticket_created_by_user_id] ON [tickets].[ticket] ([created_by_user_id])
GO
