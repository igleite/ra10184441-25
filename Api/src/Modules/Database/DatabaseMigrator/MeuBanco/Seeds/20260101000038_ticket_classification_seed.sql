DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

DECLARE @OrganizationId UNIQUEIDENTIFIER = '45143ad7-dc93-4664-982e-b93c7feb5f75';

INSERT INTO [tickets].[ticket_classification] ( [id]
                                              , [created_at]
                                              , [updated_at]
                                              , [organization_id]
                                              , [inactived_at]
                                              , [name]
                                              , [code])
VALUES ( 'abd9df9e-17d2-5283-9ba5-fdc79f1ec90e'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 1'
       , N'CLS001')

     , ( '37e01cb1-f101-4bcf-a8fb-9cfdf79cac54'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 2'
       , N'CLS002')

     , ( '4eb6d170-19c2-a5a0-d5d6-4c3d5fa40fac'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 3'
       , N'CLS003')

     , ( '84f28be0-5954-7b04-f49e-517500234c97'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 4'
       , N'CLS004')

     , ( '025889de-2ba4-9aef-62b0-961cfe7d4aa9'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 5'
       , N'CLS005')

     , ( '5c0d6cab-1e8d-1e13-b8f0-b45929920561'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 6'
       , N'CLS006')

     , ( 'a7a84074-a5a3-7994-abd3-b4150ad55b3a'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 7'
       , N'CLS007')

     , ( '9ab8444a-821d-d26c-df4a-dcfc896d148a'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 8'
       , N'CLS008')

     , ( '759a6b7a-9c8a-721d-a09b-4b3e949d13c4'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 9'
       , N'CLS009')

     , ( 'ea2307ee-80c2-51ab-322c-8592e62f0456'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 10'
       , N'CLS010')

     , ( '6ae19046-cd19-96b9-fad5-4941f0e0ec0d'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 11'
       , N'CLS011')

     , ( 'be129733-204d-f141-3651-446af052e67c'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 12'
       , N'CLS012')

     , ( 'f1d29efc-0117-883c-60b0-60999ae02af1'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 13'
       , N'CLS013')

     , ( '973fa553-3f47-b3ee-bb58-aa00589eb9ab'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 14'
       , N'CLS014')

     , ( 'a707edb8-094c-c890-1b8d-54f80d1a7264'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 15'
       , N'CLS015')

     , ( '3ddbfcc5-e29f-81af-8cf1-dc06cd5e4306'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 16'
       , N'CLS016')

     , ( 'b3fc2646-ba89-5723-c6b0-41a6f05cdd41'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 17'
       , N'CLS017')

     , ( '10ae6906-8e54-3d19-e70c-1cdb5855619f'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 18'
       , N'CLS018')

     , ( '1e87c362-dffa-5456-3bd7-84f3d76d7d17'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 19'
       , N'CLS019')

     , ( '75a08633-4d8d-ddb3-a627-91f637101b9d'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Classification 20'
       , N'CLS020');
GO
