DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

DECLARE @OrganizationId UNIQUEIDENTIFIER = '45143ad7-dc93-4664-982e-b93c7feb5f75';

INSERT INTO [tenants].[organization_user] ( [id]
                                          , [created_at]
                                          , [updated_at]
                                          , [organization_id]
                                          , [inactived_at]
                                          , [user_id]
                                          , [team_id])
VALUES ( 'c10feb2b-fd6c-61ca-ea9b-8df5d549df45'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '6a508a12-6e9b-4ef6-bdb9-8372347cb8a8'
       , '592b24f8-cd88-641d-8994-f669e5ac59e9')

     , ( 'cc97f6c2-ab1b-2418-7161-9a0fd0f2ec11'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '820a63c4-5e10-0664-6a32-a1325293b324'
       , 'dc8900d5-5740-2a1c-66a4-2f6346cd5900')

     , ( '4318098c-648c-038c-155b-062e214fea40'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'f07de8da-65f0-b9ae-e5c3-ea19e53455c7'
       , '1c8cdab0-b7b7-0dc3-19de-1a4a05c155a8')

     , ( '450381c9-6359-844d-e1b3-286a9b33cda6'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'd8c4f403-b7c4-e127-b16f-61be2be2a0d2'
       , '62f863d1-4d1d-90b5-fc0f-793ce7a08872')

     , ( '1a34e719-c310-80ab-c7a3-6094dcda6cb0'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'da8b873b-162a-90ff-f677-b6a4fa163e30'
       , 'b736f8b0-8eff-e965-e3f1-5509a4ca9da0')

     , ( 'e766316e-56a5-37e1-0cac-7b6deab4c3cb'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '52523534-aaf4-3908-039d-b1aaa51869d5'
       , '5c67a23f-7f4e-38fd-06a6-0e127d62dad0')

     , ( 'cfc510ec-2057-f93c-1cca-590e1c2af7a1'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'c506176a-d7ee-099a-f97b-0a5d2a7cd3cf'
       , '5c254cea-7154-6f6a-89af-4e7623b2c17b')

     , ( '0431cf82-b348-306f-f873-33ddc4a3ad07'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'afa46c7d-18e6-9b8e-6d2c-37d47fd0fc60'
       , '14608ae2-45b4-393c-1bb6-e895b652df4d')

     , ( '88098f88-555f-ff65-0950-ed7e520a746c'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'dee9c1a7-b868-04d9-854a-3140532702ad'
       , 'd9f15e71-9ecf-6715-0c58-0fe84e335dc0')

     , ( 'd3b064ca-0380-54aa-e64d-d1ae7324a61e'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '823373be-7d4b-9d3c-a8cc-e66e44c89770'
       , '7012c2a2-d932-414f-cd20-2a09974804a4')

     , ( '4d79bfbc-f45d-a7df-c2b3-a04af311b1ad'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '6d9a0e5f-3057-745e-0965-dc42108f5c64'
       , '73a3316e-3fb2-8fc7-00f6-c7c1b725da7e')

     , ( '124dc7ec-5422-f1bf-43b4-0e0aadcb69c1'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '98dbfdbf-3658-91c3-bffa-7989d925e4cf'
       , 'f842a674-65e7-890e-3bfd-9563b5f9dd28')

     , ( 'ee390d4c-fcdb-869b-4305-e9a3bc0580d1'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'cd2f0b75-7f78-43c0-5563-273a72f27c14'
       , '6a094c81-956a-baf0-3c5c-6f058152965e')

     , ( '71de3c88-c59d-9b83-1528-4e9366170581'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '7f85c379-479b-286e-3c75-ff02ef8a49c0'
       , '8396321d-11d2-b038-c5df-ada4e4b2a46d')

     , ( 'd12b37f4-8c31-feb4-0647-c11c59e62d92'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'fe7659c2-46fa-e871-a9de-b8dbaf42945a'
       , '11e8d7a4-6e1e-af44-5b19-c7335c60c68c')

     , ( 'a450e834-7fbc-fa72-9125-68d180f84493'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'f48021ef-7efd-3215-0e10-b583b7d0e77d'
       , 'c6a5eaaa-26f7-6abf-496b-41752a0537db')

     , ( 'd7b1e0c9-afe5-635b-e57b-402c46c31200'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '57150519-4ba7-7d1a-0ae7-dfd35091c24c'
       , '5a400120-14bf-a4e4-8cb0-0801c166291c')

     , ( '5b3aed02-536f-71ba-a5d0-6d2418d7c2d4'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '8e640d6c-6393-d989-a33f-6cc546d5422a'
       , '838f9dda-c8b1-ffa5-8c19-dded4b6984b6')

     , ( '6c8bbefa-9943-9bfd-0e95-4ad8f350cee2'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '61198024-c77b-40bd-7df6-dca2cd57ba00'
       , '194b5158-cb1a-b50f-b23d-d5b0c43fad92')

     , ( 'ebda3d83-fc09-d1cf-abdb-686935a4a7e0'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '560b5dcb-f37a-b5ba-ca8f-1fc8dcfe7073'
       , '1389c96d-471c-91ed-1743-ef2ac3a14e38');

INSERT INTO [tenants].[customer_user] ( [id]
                                      , [created_at]
                                      , [updated_at]
                                      , [organization_id]
                                      , [inactived_at]
                                      , [customer_id]
                                      , [user_id]
                                      , [role_id])
VALUES ( 'ed636d59-4f4e-499a-6e82-adacd8ca98e7'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '9d673361-ea55-292c-89f9-b4f94b676aca'
       , '6a508a12-6e9b-4ef6-bdb9-8372347cb8a8'
       , '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5')

     , ( 'ddb05893-4433-6b95-6956-58304d41e1be'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '143c7bfd-ac9e-e560-0886-7c864052a22d'
       , '820a63c4-5e10-0664-6a32-a1325293b324'
       , '11fe3aaa-d36d-4cae-9bce-7c5084360425')

     , ( 'b1cb2df8-8101-fb6e-a007-c640d23d250b'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'ef5ea879-764c-0bcc-a478-4ddbeceac20f'
       , 'f07de8da-65f0-b9ae-e5c3-ea19e53455c7'
       , '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5')

     , ( '4cbe03fb-6a42-da78-ba57-3d4bf1d19593'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'ea0ead6d-1b7d-1ca9-092b-8193dae74490'
       , 'd8c4f403-b7c4-e127-b16f-61be2be2a0d2'
       , '11fe3aaa-d36d-4cae-9bce-7c5084360425')

     , ( 'f8a41592-f1ea-4275-d6f2-823decae1a6c'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '15c041f1-3cd4-7d83-9d7c-52861ddea0dc'
       , 'da8b873b-162a-90ff-f677-b6a4fa163e30'
       , '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5')

     , ( '7be9e3e4-a588-59f9-3d4c-b52a9c5c97dd'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '8d23ef19-7347-329b-28b5-e4c76592e312'
       , '52523534-aaf4-3908-039d-b1aaa51869d5'
       , '11fe3aaa-d36d-4cae-9bce-7c5084360425')

     , ( 'b792b760-fbbf-50ca-5362-820592bbab40'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'b55cad2e-4f3f-40a2-a027-370e347b471d'
       , 'c506176a-d7ee-099a-f97b-0a5d2a7cd3cf'
       , '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5')

     , ( '8c9b4240-7e07-857a-8cef-acf8b23626ee'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'd996f279-074b-a62f-2522-0707c6edadb4'
       , 'afa46c7d-18e6-9b8e-6d2c-37d47fd0fc60'
       , '11fe3aaa-d36d-4cae-9bce-7c5084360425')

     , ( 'a908b2e1-0009-4000-8000-000000000009'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '2eda5200-62d5-4faa-760c-42e6bb4508ff'
       , 'dee9c1a7-b868-04d9-854a-3140532702ad'
       , '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5')

     , ( 'b908b2e1-0010-4000-8000-000000000010'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'ef70def3-c3e9-1ffa-1c02-9656422c6094'
       , '823373be-7d4b-9d3c-a8cc-e66e44c89770'
       , '11fe3aaa-d36d-4cae-9bce-7c5084360425')

     , ( 'c908b2e1-0011-4000-8000-000000000011'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '7d4de21e-9bd3-e168-01ff-c55681d5ca81'
       , '6d9a0e5f-3057-745e-0965-dc42108f5c64'
       , '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5')

     , ( 'd908b2e1-0012-4000-8000-000000000012'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '9e6c1e40-7ea1-ab06-f893-d3b76c8a9f78'
       , '98dbfdbf-3658-91c3-bffa-7989d925e4cf'
       , '11fe3aaa-d36d-4cae-9bce-7c5084360425')

     , ( 'e908b2e1-0013-4000-8000-000000000013'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'da5cd6ee-a072-722d-88a9-e5d1d37fcb08'
       , 'cd2f0b75-7f78-43c0-5563-273a72f27c14'
       , '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5')

     , ( 'f908b2e1-0014-4000-8000-000000000014'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '1d38ccd0-095f-dc2a-54cc-731bc13b72f8'
       , '7f85c379-479b-286e-3c75-ff02ef8a49c0'
       , '11fe3aaa-d36d-4cae-9bce-7c5084360425')

     , ( '0908b2e1-0015-4000-8000-000000000015'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '16c1c8ce-6c07-2dfc-0606-879ed9f19f09'
       , 'fe7659c2-46fa-e871-a9de-b8dbaf42945a'
       , '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5')

     , ( '0a08b2e1-0016-4000-8000-000000000016'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '292490c3-8064-677b-0eaa-ac803ead2a3d'
       , 'f48021ef-7efd-3215-0e10-b583b7d0e77d'
       , '11fe3aaa-d36d-4cae-9bce-7c5084360425')

     , ( '0b08b2e1-0017-4000-8000-000000000017'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '00c32237-56f3-5185-61c9-95b7fe92f4c8'
       , '57150519-4ba7-7d1a-0ae7-dfd35091c24c'
       , '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5')

     , ( '0c08b2e1-0018-4000-8000-000000000018'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'de355b11-7d2a-d2c7-eb52-c3896fe13a2d'
       , '8e640d6c-6393-d989-a33f-6cc546d5422a'
       , '11fe3aaa-d36d-4cae-9bce-7c5084360425')

     , ( '0d08b2e1-0019-4000-8000-000000000019'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '88c752a4-c238-787d-c781-4c15a4e4a190'
       , '61198024-c77b-40bd-7df6-dca2cd57ba00'
       , '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5')

     , ( '0e08b2e1-0020-4000-8000-000000000020'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '569ef6ef-16cf-aa2a-f78a-6620f903abab'
       , '560b5dcb-f37a-b5ba-ca8f-1fc8dcfe7073'
       , '11fe3aaa-d36d-4cae-9bce-7c5084360425');
GO
