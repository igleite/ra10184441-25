DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

INSERT INTO [feature_flags].[feature_flag] ( [id]
                                           , [created_at]
                                           , [updated_at]
                                           , [name]
                                           , [description]
                                           , [value])
VALUES ( 'f4cb2236-5849-ff1c-328a-a14069abb9c1'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag1'
       , N'Seed feature flag 1'
       , 1)

     , ( '444f9e66-f399-4015-b123-8d91013b09ab'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag2'
       , N'Seed feature flag 2'
       , 0)

     , ( '2d09cc4b-4bdb-35cd-c5c2-58e30958c09e'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag3'
       , N'Seed feature flag 3'
       , 1)

     , ( 'c533ee85-06a4-d88d-e750-1ab05507e25f'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag4'
       , N'Seed feature flag 4'
       , 0)

     , ( 'e6cc5dce-8f01-c3c7-5164-0cf5616020dd'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag5'
       , N'Seed feature flag 5'
       , 1)

     , ( 'b81637a8-1f02-0c49-b939-730107d2bdb1'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag6'
       , N'Seed feature flag 6'
       , 0)

     , ( '62cb7ea8-eced-5644-bb12-8bea5e7c529a'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag7'
       , N'Seed feature flag 7'
       , 1)

     , ( 'ae0da455-7453-df59-df88-130083986ada'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag8'
       , N'Seed feature flag 8'
       , 0)

     , ( 'c73d33f2-837d-3d6d-7045-a9f4abd7b5ed'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag9'
       , N'Seed feature flag 9'
       , 1)

     , ( '73ae45e9-33d0-ac10-7bdc-6408b93dd26c'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag10'
       , N'Seed feature flag 10'
       , 0)

     , ( '0eee15a9-978e-82d4-615a-9525c63cc5e8'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag11'
       , N'Seed feature flag 11'
       , 1)

     , ( '0269bb15-8d22-a940-4285-d311873d1614'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag12'
       , N'Seed feature flag 12'
       , 0)

     , ( 'e07e69c7-9f5e-ec9f-6b34-4298d9147107'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag13'
       , N'Seed feature flag 13'
       , 1)

     , ( '113dd119-a040-6249-0c72-810bd77f4a8e'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag14'
       , N'Seed feature flag 14'
       , 0)

     , ( '66408ac8-0379-c8d9-f3ee-791d51eaed22'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag15'
       , N'Seed feature flag 15'
       , 1)

     , ( '91d9d67c-7ba5-e633-744d-a30ca389edef'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag16'
       , N'Seed feature flag 16'
       , 0)

     , ( 'bf4ffa14-a0dc-7f38-856e-993fa4a27afc'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag17'
       , N'Seed feature flag 17'
       , 1)

     , ( 'eff98ae0-a571-8e0a-9775-8dfad299d5fd'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag18'
       , N'Seed feature flag 18'
       , 0)

     , ( '5ede4842-edea-76c2-e67f-fd7d27b0f65a'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag19'
       , N'Seed feature flag 19'
       , 1)

     , ( '3149c9be-1d2e-c880-de72-d48050903cd1'
       , @CurrentDate
       , @CurrentDate
       , N'FeatureFlag20'
       , N'Seed feature flag 20'
       , 0);
GO
