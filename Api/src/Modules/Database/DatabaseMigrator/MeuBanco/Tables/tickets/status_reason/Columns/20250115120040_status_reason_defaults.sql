ALTER TABLE [tickets].[status_reason]
    ADD CONSTRAINT [DF_status_reason_is_opening_default] DEFAULT 0 FOR [is_opening_default]
GO
