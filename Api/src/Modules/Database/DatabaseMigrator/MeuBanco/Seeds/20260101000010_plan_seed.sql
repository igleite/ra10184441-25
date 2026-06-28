DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

INSERT INTO [tenants].[plan] ( [id]
                             , [created_at]
                             , [updated_at]
                             , [inactived_at]
                             , [name]
                             , [description]
                             , [max_users]
                             , [max_clients]
                             , [max_tickets])
VALUES ( '4dda0cb7-c327-4669-b657-e12ffcaf7205'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 1'
       , N'Seed plan 1'
       , 100
       , 100
       , 1000)

     , ( '5db6cb1b-9e70-4948-9796-9e964471e4ca'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 2'
       , N'Seed plan 2'
       , 100
       , 100
       , 1000)

     , ( 'b889e3c1-3a54-413e-b448-1338ec3106fe'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 3'
       , N'Seed plan 3'
       , 100
       , 100
       , 1000)

     , ( '5793fcb0-2bfa-458c-a4ca-c4645b398d99'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 4'
       , N'Seed plan 4'
       , 100
       , 100
       , 1000)

     , ( '0df8934d-042a-4b71-ad39-71d5396f3f82'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 5'
       , N'Seed plan 5'
       , 100
       , 100
       , 1000)

     , ( '91cebed6-b24d-4989-bbe6-a758c2f56397'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 6'
       , N'Seed plan 6'
       , 100
       , 100
       , 1000)

     , ( 'f79bc96c-6a71-4d43-bfc0-ca940a89d38b'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 7'
       , N'Seed plan 7'
       , 100
       , 100
       , 1000)

     , ( '6935a487-6bfe-486f-a10a-4161ba55cddb'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 8'
       , N'Seed plan 8'
       , 100
       , 100
       , 1000)

     , ( '5991ff02-1140-47c2-a368-8a536f3882cf'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 9'
       , N'Seed plan 9'
       , 100
       , 100
       , 1000)

     , ( '18249302-9e97-4759-b10e-408e4d28ded0'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 10'
       , N'Seed plan 10'
       , 100
       , 100
       , 1000)

     , ( '6db306c0-ee75-4a33-a7b7-9075ddf32903'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 11'
       , N'Seed plan 11'
       , 100
       , 100
       , 1000)

     , ( '09a0d140-5036-4396-8939-2650dbbacc14'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 12'
       , N'Seed plan 12'
       , 100
       , 100
       , 1000)

     , ( '61e8f604-4392-4dd0-91ec-9792ebfc16b4'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 13'
       , N'Seed plan 13'
       , 100
       , 100
       , 1000)

     , ( '3cdc168a-61b2-4f68-bcd0-83531f4d8fcb'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 14'
       , N'Seed plan 14'
       , 100
       , 100
       , 1000)

     , ( '1e3abdb2-2163-46af-88b0-9797f74c80e5'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 15'
       , N'Seed plan 15'
       , 100
       , 100
       , 1000)

     , ( '8d236877-c42d-4982-9716-238f9b45a57e'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 16'
       , N'Seed plan 16'
       , 100
       , 100
       , 1000)

     , ( '07d4dd6c-c41a-4b01-b066-7beff9476f73'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 17'
       , N'Seed plan 17'
       , 100
       , 100
       , 1000)

     , ( '63122cdc-3ff7-45c9-9c91-102d460506bd'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 18'
       , N'Seed plan 18'
       , 100
       , 100
       , 1000)

     , ( '7e0ed9fb-2a22-436b-a8b6-2c6de78246a3'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 19'
       , N'Seed plan 19'
       , 100
       , 100
       , 1000)

     , ( 'e6ebfb64-6fda-4f21-8e19-caa6e4614b2b'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , N'Plan 20'
       , N'Seed plan 20'
       , 100
       , 100
       , 1000);
GO
