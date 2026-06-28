DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

INSERT INTO [tenants].[user] ( [id]
                             , [created_at]
                             , [updated_at]
                             , [inactived_at]
                             , [name]
                             , [email]
                             , [role_id])
VALUES ( '3f3816db-5580-4039-91c3-ddca5d51970c'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Super Admin'
       , N'superadmin@example.com'
       , '8f42805c-d971-4fdb-b099-d69c6dc6b2a4')

     , ( '6a508a12-6e9b-4ef6-bdb9-8372347cb8a8'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 1'
       , N'user01@example.com'
       , NULL)

     , ( '820a63c4-5e10-0664-6a32-a1325293b324'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 2'
       , N'user02@example.com'
       , NULL)

     , ( 'f07de8da-65f0-b9ae-e5c3-ea19e53455c7'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 3'
       , N'user03@example.com'
       , NULL)

     , ( 'd8c4f403-b7c4-e127-b16f-61be2be2a0d2'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 4'
       , N'user04@example.com'
       , NULL)

     , ( 'da8b873b-162a-90ff-f677-b6a4fa163e30'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 5'
       , N'user05@example.com'
       , NULL)

     , ( '52523534-aaf4-3908-039d-b1aaa51869d5'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 6'
       , N'user06@example.com'
       , NULL)

     , ( 'c506176a-d7ee-099a-f97b-0a5d2a7cd3cf'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 7'
       , N'user07@example.com'
       , NULL)

     , ( 'afa46c7d-18e6-9b8e-6d2c-37d47fd0fc60'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 8'
       , N'user08@example.com'
       , NULL)

     , ( 'dee9c1a7-b868-04d9-854a-3140532702ad'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 9'
       , N'user09@example.com'
       , NULL)

     , ( '823373be-7d4b-9d3c-a8cc-e66e44c89770'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 10'
       , N'user10@example.com'
       , NULL)

     , ( '6d9a0e5f-3057-745e-0965-dc42108f5c64'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 11'
       , N'user11@example.com'
       , NULL)

     , ( '98dbfdbf-3658-91c3-bffa-7989d925e4cf'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 12'
       , N'user12@example.com'
       , NULL)

     , ( 'cd2f0b75-7f78-43c0-5563-273a72f27c14'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 13'
       , N'user13@example.com'
       , NULL)

     , ( '7f85c379-479b-286e-3c75-ff02ef8a49c0'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 14'
       , N'user14@example.com'
       , NULL)

     , ( 'fe7659c2-46fa-e871-a9de-b8dbaf42945a'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 15'
       , N'user15@example.com'
       , NULL)

     , ( 'f48021ef-7efd-3215-0e10-b583b7d0e77d'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 16'
       , N'user16@example.com'
       , NULL)

     , ( '57150519-4ba7-7d1a-0ae7-dfd35091c24c'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 17'
       , N'user17@example.com'
       , NULL)

     , ( '8e640d6c-6393-d989-a33f-6cc546d5422a'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 18'
       , N'user18@example.com'
       , NULL)

     , ( '61198024-c77b-40bd-7df6-dca2cd57ba00'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 19'
       , N'user19@example.com'
       , NULL)

     , ( '560b5dcb-f37a-b5ba-ca8f-1fc8dcfe7073'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 20'
       , N'user20@example.com'
       , NULL)

     , ( '3174fbfd-18c8-2e48-855d-efb2541fcc07'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'User 21'
       , N'user21@example.com'
       , NULL);
GO
