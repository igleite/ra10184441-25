DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

INSERT INTO [tickets].[chat] ( [id]
                             , [created_at]
                             , [updated_at]
                             , [deleted_at]
                             , [ticket_id]
                             , [user_id]
                             , [message])
VALUES ( 'ae7dd396-e316-1cbb-ddd4-5ff269bc1749'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '1290594f-25a7-c5ef-a4a8-5125b6ab5d7e'
       , '6a508a12-6e9b-4ef6-bdb9-8372347cb8a8'
       , N'Chat message for ticket 1')

     , ( '0f00e384-4dce-1e67-c7f9-455e9e21f5db'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'b7b054fb-8ba2-e7f0-0cf2-739f294cf422'
       , '820a63c4-5e10-0664-6a32-a1325293b324'
       , N'Chat message for ticket 2')

     , ( '29dd6ffd-59b4-74b8-8e67-bf68073e1007'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'f27ca092-1b6f-2d9e-6f04-3e8c09c89aee'
       , 'f07de8da-65f0-b9ae-e5c3-ea19e53455c7'
       , N'Chat message for ticket 3')

     , ( '44f7b0df-ff7d-ec74-83a2-9ad05eab0078'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '4020d31c-bdb2-7e02-857d-2ec7277351c8'
       , 'd8c4f403-b7c4-e127-b16f-61be2be2a0d2'
       , N'Chat message for ticket 4')

     , ( '186b66b6-80fe-4aed-8338-dd0cf3d1dce1'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '9f54940a-4ba0-857d-54ef-ab91480de079'
       , 'da8b873b-162a-90ff-f677-b6a4fa163e30'
       , N'Chat message for ticket 5')

     , ( 'e5763899-2eaa-011b-c198-83b9e8c37ba4'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '98c582e9-82e0-5a1b-da84-345356edfb66'
       , '52523534-aaf4-3908-039d-b1aaa51869d5'
       , N'Chat message for ticket 6')

     , ( '960807dd-7b4e-c4be-1300-8ce72625fe64'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '9196d9b1-e1be-33ca-d502-ed190edcab71'
       , 'c506176a-d7ee-099a-f97b-0a5d2a7cd3cf'
       , N'Chat message for ticket 7')

     , ( 'e503f882-0840-cc49-62ca-fb47629fd31d'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '4b4366f5-551e-6a12-bed6-4bf21b94b11a'
       , 'afa46c7d-18e6-9b8e-6d2c-37d47fd0fc60'
       , N'Chat message for ticket 8')

     , ( '1bc7e2cf-5d9c-4a14-b238-cea3a3ff70b9'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '4eb15c8d-83e3-a815-46ff-53f9a794749f'
       , 'dee9c1a7-b868-04d9-854a-3140532702ad'
       , N'Chat message for ticket 9')

     , ( '234df413-c2b4-aa81-e853-9f6c8078b925'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'f4aa6446-017f-1d09-1a46-cfed476a8ee4'
       , '823373be-7d4b-9d3c-a8cc-e66e44c89770'
       , N'Chat message for ticket 10')

     , ( '4fe87d4d-6e3c-5b28-bafa-d2567d281b49'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'b01145ab-0abb-c3e4-c454-afbc16b43cba'
       , '6d9a0e5f-3057-745e-0965-dc42108f5c64'
       , N'Chat message for ticket 11')

     , ( '0449aef1-558c-ebf6-95e1-9b794eb85d74'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '2be0feb9-473c-6635-3c3f-b5fef79b56a3'
       , '98dbfdbf-3658-91c3-bffa-7989d925e4cf'
       , N'Chat message for ticket 12')

     , ( 'c659bf68-07aa-aa48-10c8-272607ff45fa'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '25bb58ce-5c82-f89d-5ebd-899209a7eb7c'
       , 'cd2f0b75-7f78-43c0-5563-273a72f27c14'
       , N'Chat message for ticket 13')

     , ( '4fe2973f-63cc-c862-24be-53307783d70e'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '12269959-764c-ba81-8996-dc928ae0494e'
       , '7f85c379-479b-286e-3c75-ff02ef8a49c0'
       , N'Chat message for ticket 14')

     , ( '95001c31-f4a8-c7e1-0e85-b30fc0121933'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '7f855099-d913-6d25-22cd-eb09910bef59'
       , 'fe7659c2-46fa-e871-a9de-b8dbaf42945a'
       , N'Chat message for ticket 15')

     , ( 'ed70221d-cd4b-5659-dd0d-f0ae228fac86'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '62e709d7-cfe7-8a4e-78c9-aa74a99544c6'
       , 'f48021ef-7efd-3215-0e10-b583b7d0e77d'
       , N'Chat message for ticket 16')

     , ( 'd3931a49-8eb7-d395-1bbe-ee6606744cd9'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '4cf4f120-1027-ff9f-87b8-3b94f5fe1fe1'
       , '57150519-4ba7-7d1a-0ae7-dfd35091c24c'
       , N'Chat message for ticket 17')

     , ( 'f27a4203-3253-bcce-c764-39c3fbeb0650'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '56c2af43-fb29-d3d4-db17-112f979c19c5'
       , '8e640d6c-6393-d989-a33f-6cc546d5422a'
       , N'Chat message for ticket 18')

     , ( '5694cf89-c8c4-348f-b777-369cc52bfd89'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '32ac46b7-a0b2-eb18-a576-cbb3828d0c94'
       , '61198024-c77b-40bd-7df6-dca2cd57ba00'
       , N'Chat message for ticket 19')

     , ( '849c9e58-16d9-3593-af5a-d3caf72f2d3c'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '1dada6ba-4e20-b721-53a7-1158a674ead7'
       , '560b5dcb-f37a-b5ba-ca8f-1fc8dcfe7073'
       , N'Chat message for ticket 20');
GO
