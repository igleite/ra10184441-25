ALTER TABLE [feature_flags].[feature_flag]
    ADD CONSTRAINT [DF_feature_flag_value] DEFAULT 0 FOR [value]
GO
