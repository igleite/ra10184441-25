DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

DECLARE @OrganizationId UNIQUEIDENTIFIER = '45143ad7-dc93-4664-982e-b93c7feb5f75';
DECLARE @CreatedByUserId UNIQUEIDENTIFIER = '6a508a12-6e9b-4ef6-bdb9-8372347cb8a8';

INSERT INTO [tickets].[ticket] ( [id]
                               , [created_at]
                               , [updated_at]
                               , [organization_id]
                               , [deleted_at]
                               , [customer_id]
                               , [artifact_id]
                               , [status_id]
                               , [classification_id]
                               , [allocation_center]
                               , [created_by_user_id]
                               , [description]
                               , [resolution])
VALUES ( '1290594f-25a7-c5ef-a4a8-5125b6ab5d7e'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '9d673361-ea55-292c-89f9-b4f94b676aca'
       , '4f6ac6cf-e672-8ed7-c00c-2715e75869d2'
       , '9b636342-19a7-680c-cc2a-20520ccaec1b'
       , 'abd9df9e-17d2-5283-9ba5-fdc79f1ec90e'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 1'
       , NULL)

     , ( 'b7b054fb-8ba2-e7f0-0cf2-739f294cf422'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '143c7bfd-ac9e-e560-0886-7c864052a22d'
       , 'ceb87934-f577-0ba7-2421-b84c49efb9a3'
       , 'de1c99a0-d9ab-2d71-2002-51a6634882f1'
       , '37e01cb1-f101-4bcf-a8fb-9cfdf79cac54'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 2'
       , NULL)

     , ( 'f27ca092-1b6f-2d9e-6f04-3e8c09c89aee'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'ef5ea879-764c-0bcc-a478-4ddbeceac20f'
       , '9d8c1019-6db5-fc51-e5a7-50a2e2ba3325'
       , 'a11a2f62-e77a-12ff-0802-79a3a6eca230'
       , '4eb6d170-19c2-a5a0-d5d6-4c3d5fa40fac'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 3'
       , NULL)

     , ( '4020d31c-bdb2-7e02-857d-2ec7277351c8'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'ea0ead6d-1b7d-1ca9-092b-8193dae74490'
       , 'd7859390-3f6b-0714-c7d1-797d12e72909'
       , '7873dbd8-729e-8f8e-593d-84c5a026d7a1'
       , '84f28be0-5954-7b04-f49e-517500234c97'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 4'
       , N'Resolved in seed 4')

     , ( '9f54940a-4ba0-857d-54ef-ab91480de079'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '15c041f1-3cd4-7d83-9d7c-52861ddea0dc'
       , '489dc813-3e8f-e2be-7cd0-bb380cdce886'
       , 'dcfcfe82-3865-b003-7b1e-c9107d304297'
       , '025889de-2ba4-9aef-62b0-961cfe7d4aa9'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 5'
       , NULL)

     , ( '98c582e9-82e0-5a1b-da84-345356edfb66'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '8d23ef19-7347-329b-28b5-e4c76592e312'
       , 'b4635e55-fe9b-5ea7-7cc4-87cf9a5acd34'
       , 'c56341ed-7f75-41ed-4541-3f0c4307c4f6'
       , '5c0d6cab-1e8d-1e13-b8f0-b45929920561'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 6'
       , NULL)

     , ( '9196d9b1-e1be-33ca-d502-ed190edcab71'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'b55cad2e-4f3f-40a2-a027-370e347b471d'
       , '8c0382b1-cca6-4fb5-4385-8b702a751abd'
       , '7ed42ede-8898-a688-5a68-f8680f0b40c6'
       , 'a7a84074-a5a3-7994-abd3-b4150ad55b3a'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 7'
       , NULL)

     , ( '4b4366f5-551e-6a12-bed6-4bf21b94b11a'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'd996f279-074b-a62f-2522-0707c6edadb4'
       , 'a9fde140-1c8a-cbd4-2bec-d37c3e6664b8'
       , '9f2b3697-db48-a017-dd65-d6c806a9ed88'
       , '9ab8444a-821d-d26c-df4a-dcfc896d148a'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 8'
       , N'Resolved in seed 8')

     , ( '4eb15c8d-83e3-a815-46ff-53f9a794749f'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '2eda5200-62d5-4faa-760c-42e6bb4508ff'
       , 'de2e7751-c3b1-281a-15bc-f6ad17f044c6'
       , '13ed6d91-f8f1-6161-14a4-7b438720a011'
       , '759a6b7a-9c8a-721d-a09b-4b3e949d13c4'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 9'
       , NULL)

     , ( 'f4aa6446-017f-1d09-1a46-cfed476a8ee4'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'ef70def3-c3e9-1ffa-1c02-9656422c6094'
       , '6ab72e9b-5dda-1709-fbef-7f2ab2bbff69'
       , 'd462ce9c-af27-e407-7a37-be99a6d72269'
       , 'ea2307ee-80c2-51ab-322c-8592e62f0456'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 10'
       , NULL)

     , ( 'b01145ab-0abb-c3e4-c454-afbc16b43cba'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '7d4de21e-9bd3-e168-01ff-c55681d5ca81'
       , 'beb97496-3065-c282-f9a9-d9c0d7a1734a'
       , '6637848d-f887-f4f4-3f74-fd9200dfff47'
       , '6ae19046-cd19-96b9-fad5-4941f0e0ec0d'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 11'
       , NULL)

     , ( '2be0feb9-473c-6635-3c3f-b5fef79b56a3'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '9e6c1e40-7ea1-ab06-f893-d3b76c8a9f78'
       , '1c2ff3f6-ec10-8260-a8e7-75b8e825f8aa'
       , '751decbb-dacc-7dde-0bd6-41e762fa23e5'
       , 'be129733-204d-f141-3651-446af052e67c'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 12'
       , N'Resolved in seed 12')

     , ( '25bb58ce-5c82-f89d-5ebd-899209a7eb7c'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'da5cd6ee-a072-722d-88a9-e5d1d37fcb08'
       , '2850ff70-75f9-5162-6a47-91e9692a3316'
       , '4a50cbf3-a6c9-c471-b053-848dd0ba89e6'
       , 'f1d29efc-0117-883c-60b0-60999ae02af1'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 13'
       , NULL)

     , ( '12269959-764c-ba81-8996-dc928ae0494e'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '1d38ccd0-095f-dc2a-54cc-731bc13b72f8'
       , '6f58049e-4c26-7b26-808c-a59116b90331'
       , 'f1446da2-98ca-6506-0aaa-d0d4e7d5db4b'
       , '973fa553-3f47-b3ee-bb58-aa00589eb9ab'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 14'
       , NULL)

     , ( '7f855099-d913-6d25-22cd-eb09910bef59'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '16c1c8ce-6c07-2dfc-0606-879ed9f19f09'
       , 'f45dd17e-65c9-6896-867f-3f1931eeb15c'
       , 'b43cf2cb-5afb-0362-0c91-3bff4547c428'
       , 'a707edb8-094c-c890-1b8d-54f80d1a7264'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 15'
       , NULL)

     , ( '62e709d7-cfe7-8a4e-78c9-aa74a99544c6'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '292490c3-8064-677b-0eaa-ac803ead2a3d'
       , '32dd8e09-55cf-9d06-c522-cc4a7ac36120'
       , 'f05051ef-c675-b069-1a88-63e067bbf47b'
       , '3ddbfcc5-e29f-81af-8cf1-dc06cd5e4306'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 16'
       , N'Resolved in seed 16')

     , ( '4cf4f120-1027-ff9f-87b8-3b94f5fe1fe1'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '00c32237-56f3-5185-61c9-95b7fe92f4c8'
       , '677fddbe-ea69-c08d-f4d7-316cf4d6c724'
       , 'b3534f8d-533a-9b99-8e52-5c9366c6a66f'
       , 'b3fc2646-ba89-5723-c6b0-41a6f05cdd41'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 17'
       , NULL)

     , ( '56c2af43-fb29-d3d4-db17-112f979c19c5'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 'de355b11-7d2a-d2c7-eb52-c3896fe13a2d'
       , '14f2a37c-8111-0463-12ca-f04b2c34ccae'
       , 'f8674b5e-a695-885c-01e8-b564bc361384'
       , '10ae6906-8e54-3d19-e70c-1cdb5855619f'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 18'
       , NULL)

     , ( '32ac46b7-a0b2-eb18-a576-cbb3828d0c94'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '88c752a4-c238-787d-c781-4c15a4e4a190'
       , 'c2d2d9ae-163a-40d7-a4eb-be89894dd2e4'
       , '92cafa7b-ddc0-933e-78bd-9eee9a2b6fe6'
       , '1e87c362-dffa-5456-3bd7-84f3d76d7d17'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 19'
       , NULL)

     , ( '1dada6ba-4e20-b721-53a7-1158a674ead7'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , '569ef6ef-16cf-aa2a-f78a-6620f903abab'
       , 'fc6da8cd-fdff-773a-8c65-382a0925a92f'
       , 'ef2084c4-d6f0-b534-3d3e-4c67df63f5b5'
       , '75a08633-4d8d-ddb3-a627-91f637101b9d'
       , 1
       , @CreatedByUserId
       , N'Ticket seed 20'
       , N'Resolved in seed 20');
GO
