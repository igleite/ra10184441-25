DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

DECLARE @OrganizationId UNIQUEIDENTIFIER = '45143ad7-dc93-4664-982e-b93c7feb5f75';

INSERT INTO [tenants].[team] ( [id]
                             , [created_at]
                             , [updated_at]
                             , [organization_id]
                             , [inactived_at]
                             , [name]
                             , [code]
                             , [role_id])
VALUES ( '592b24f8-cd88-641d-8994-f669e5ac59e9'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Default Team'
       , N'DEFAULT_TEAM'
       , 'e597e22c-e22e-4849-928c-e919717d2b8c')

     , ( 'dc8900d5-5740-2a1c-66a4-2f6346cd5900'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 2'
       , N'TEAM001'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '1c8cdab0-b7b7-0dc3-19de-1a4a05c155a8'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 3'
       , N'TEAM002'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '62f863d1-4d1d-90b5-fc0f-793ce7a08872'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 4'
       , N'TEAM003'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( 'b736f8b0-8eff-e965-e3f1-5509a4ca9da0'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 5'
       , N'TEAM004'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '5c67a23f-7f4e-38fd-06a6-0e127d62dad0'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 6'
       , N'TEAM005'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '5c254cea-7154-6f6a-89af-4e7623b2c17b'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 7'
       , N'TEAM006'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '14608ae2-45b4-393c-1bb6-e895b652df4d'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 8'
       , N'TEAM007'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( 'd9f15e71-9ecf-6715-0c58-0fe84e335dc0'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 9'
       , N'TEAM008'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '7012c2a2-d932-414f-cd20-2a09974804a4'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 10'
       , N'TEAM009'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '73a3316e-3fb2-8fc7-00f6-c7c1b725da7e'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 11'
       , N'TEAM010'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( 'f842a674-65e7-890e-3bfd-9563b5f9dd28'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 12'
       , N'TEAM011'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '6a094c81-956a-baf0-3c5c-6f058152965e'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 13'
       , N'TEAM012'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '8396321d-11d2-b038-c5df-ada4e4b2a46d'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 14'
       , N'TEAM013'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '11e8d7a4-6e1e-af44-5b19-c7335c60c68c'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 15'
       , N'TEAM014'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( 'c6a5eaaa-26f7-6abf-496b-41752a0537db'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 16'
       , N'TEAM015'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '5a400120-14bf-a4e4-8cb0-0801c166291c'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 17'
       , N'TEAM016'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '838f9dda-c8b1-ffa5-8c19-dded4b6984b6'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 18'
       , N'TEAM017'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '194b5158-cb1a-b50f-b23d-d5b0c43fad92'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 19'
       , N'TEAM018'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d')

     , ( '1389c96d-471c-91ed-1743-ef2ac3a14e38'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Team 20'
       , N'TEAM019'
       , 'dc8c024b-69e2-4800-af50-f0fbd690279d');
GO
