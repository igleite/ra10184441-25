DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

INSERT INTO [tenants].[organization] ( [id]
                                     , [created_at]
                                     , [updated_at]
                                     , [name]
                                     , [document]
                                     , [slug])
VALUES ( '45143ad7-dc93-4664-982e-b93c7feb5f75'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 1'
       , N'01.141.900/0001-01'
       , N'ACME')

     , ( '428360aa-be01-4687-8c56-653529d2bdc3'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 2'
       , N'02.141.900/0001-02'
       , N'ORG02')

     , ( '3f51a410-47b8-8797-a59d-e0552c4b4446'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 3'
       , N'03.141.900/0001-03'
       , N'ORG03')

     , ( 'b5a05e41-b469-8b75-0bc7-06551bcef748'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 4'
       , N'04.141.900/0001-04'
       , N'ORG04')

     , ( '5108d16c-d638-c0c1-f704-5c3bf1473f7b'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 5'
       , N'05.141.900/0001-05'
       , N'ORG05')

     , ( '2ccc2d51-efd7-9512-4ed6-c5ef9f682bac'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 6'
       , N'06.141.900/0001-06'
       , N'ORG06')

     , ( '226bf114-f0b1-969f-5104-efeefd6adf18'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 7'
       , N'07.141.900/0001-07'
       , N'ORG07')

     , ( 'b5341753-c8bc-4ab9-77a2-c0d7f3a0cbfc'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 8'
       , N'08.141.900/0001-08'
       , N'ORG08')

     , ( 'f7ea2676-b6e5-7005-34f3-2269931c9ab4'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 9'
       , N'09.141.900/0001-09'
       , N'ORG09')

     , ( '2169e066-e67c-7608-64e4-a3d45321e61d'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 10'
       , N'10.141.900/0001-10'
       , N'ORG10')

     , ( '16ea0857-5899-59f7-0843-0edd5b7b68df'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 11'
       , N'11.141.900/0001-11'
       , N'ORG11')

     , ( 'fece5379-cb27-43e6-9ce5-17ecf1d2b8a9'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 12'
       , N'12.141.900/0001-12'
       , N'ORG12')

     , ( '416e4c3e-6a2e-5db5-b47b-c388db13e79c'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 13'
       , N'13.141.900/0001-13'
       , N'ORG13')

     , ( 'eeb21d90-aba6-91a7-baeb-246dac582460'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 14'
       , N'14.141.900/0001-14'
       , N'ORG14')

     , ( 'e85a8879-3edf-62be-8c41-668eb95bb860'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 15'
       , N'15.141.900/0001-15'
       , N'ORG15')

     , ( '1a21a037-a1b7-db64-7a18-3e94f799b107'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 16'
       , N'16.141.900/0001-16'
       , N'ORG16')

     , ( '87972d96-c000-f3d9-735d-fa881854bd2f'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 17'
       , N'17.141.900/0001-17'
       , N'ORG17')

     , ( '6767b1b2-1adc-67d1-e3dd-72c99ec30260'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 18'
       , N'18.141.900/0001-18'
       , N'ORG18')

     , ( '6577b06a-31d3-f449-e843-7cda34ae2e5a'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 19'
       , N'19.141.900/0001-19'
       , N'ORG19')

     , ( 'a4f93cb0-5d60-b03a-3c8a-2608fbf4cafb'
       , @CurrentDate
       , @CurrentDate
       , N'Organization 20'
       , N'20.141.900/0001-20'
       , N'ORG20');

INSERT INTO [tenants].[organization_plan] ( [id]
                                          , [created_at]
                                          , [updated_at]
                                          , [organization_id]
                                          , [inactived_at]
                                          , [plan_id]
                                          , [description]
                                          , [max_users]
                                          , [max_clients]
                                          , [max_tickets])
VALUES ( 'f85ea274-cac4-2c47-3051-1951d2f0c364'
       , @CurrentDate
       , @CurrentDate
       , '45143ad7-dc93-4664-982e-b93c7feb5f75'
       , NULL
       , '4dda0cb7-c327-4669-b657-e12ffcaf7205'
       , N'Organization 1 plan'
       , 100
       , 100
       , 1000)

     , ( '4517ae5c-c5a1-163c-d07d-8c78f8bf872e'
       , @CurrentDate
       , @CurrentDate
       , '428360aa-be01-4687-8c56-653529d2bdc3'
       , NULL
       , '5db6cb1b-9e70-4948-9796-9e964471e4ca'
       , N'Organization 2 plan'
       , 100
       , 100
       , 1000)

     , ( '46dfd203-2dc9-9707-4c1b-65679b5a3bf0'
       , @CurrentDate
       , @CurrentDate
       , '3f51a410-47b8-8797-a59d-e0552c4b4446'
       , NULL
       , 'b889e3c1-3a54-413e-b448-1338ec3106fe'
       , N'Organization 3 plan'
       , 100
       , 100
       , 1000)

     , ( '1b62508b-59fe-51c5-8702-93c6c5663413'
       , @CurrentDate
       , @CurrentDate
       , 'b5a05e41-b469-8b75-0bc7-06551bcef748'
       , NULL
       , '5793fcb0-2bfa-458c-a4ca-c4645b398d99'
       , N'Organization 4 plan'
       , 100
       , 100
       , 1000)

     , ( '427ae046-8ffd-421e-fb6e-a5711a248963'
       , @CurrentDate
       , @CurrentDate
       , '5108d16c-d638-c0c1-f704-5c3bf1473f7b'
       , NULL
       , '0df8934d-042a-4b71-ad39-71d5396f3f82'
       , N'Organization 5 plan'
       , 100
       , 100
       , 1000)

     , ( 'd5a50a49-1f01-0c70-9041-58aef6312e40'
       , @CurrentDate
       , @CurrentDate
       , '2ccc2d51-efd7-9512-4ed6-c5ef9f682bac'
       , NULL
       , '91cebed6-b24d-4989-bbe6-a758c2f56397'
       , N'Organization 6 plan'
       , 100
       , 100
       , 1000)

     , ( 'cd6c3edb-51dd-6561-9a7c-de8037e721d6'
       , @CurrentDate
       , @CurrentDate
       , '226bf114-f0b1-969f-5104-efeefd6adf18'
       , NULL
       , 'f79bc96c-6a71-4d43-bfc0-ca940a89d38b'
       , N'Organization 7 plan'
       , 100
       , 100
       , 1000)

     , ( '7658085a-1dbe-fd22-a339-855f5911215c'
       , @CurrentDate
       , @CurrentDate
       , 'b5341753-c8bc-4ab9-77a2-c0d7f3a0cbfc'
       , NULL
       , '6935a487-6bfe-486f-a10a-4161ba55cddb'
       , N'Organization 8 plan'
       , 100
       , 100
       , 1000)

     , ( '23d6264f-13bb-be37-c267-e7b73f4aa07b'
       , @CurrentDate
       , @CurrentDate
       , 'f7ea2676-b6e5-7005-34f3-2269931c9ab4'
       , NULL
       , '5991ff02-1140-47c2-a368-8a536f3882cf'
       , N'Organization 9 plan'
       , 100
       , 100
       , 1000)

     , ( 'd7bacc1c-e509-1464-e723-bf3816bab348'
       , @CurrentDate
       , @CurrentDate
       , '2169e066-e67c-7608-64e4-a3d45321e61d'
       , NULL
       , '18249302-9e97-4759-b10e-408e4d28ded0'
       , N'Organization 10 plan'
       , 100
       , 100
       , 1000)

     , ( 'ad225db3-97dd-1a59-34a6-adf980c989f9'
       , @CurrentDate
       , @CurrentDate
       , '16ea0857-5899-59f7-0843-0edd5b7b68df'
       , NULL
       , '6db306c0-ee75-4a33-a7b7-9075ddf32903'
       , N'Organization 11 plan'
       , 100
       , 100
       , 1000)

     , ( 'cdb19f6b-c492-031c-2178-b57f9ddb142d'
       , @CurrentDate
       , @CurrentDate
       , 'fece5379-cb27-43e6-9ce5-17ecf1d2b8a9'
       , NULL
       , '09a0d140-5036-4396-8939-2650dbbacc14'
       , N'Organization 12 plan'
       , 100
       , 100
       , 1000)

     , ( '96c0e5a5-dfb3-8d68-b955-e929786fc62d'
       , @CurrentDate
       , @CurrentDate
       , '416e4c3e-6a2e-5db5-b47b-c388db13e79c'
       , NULL
       , '61e8f604-4392-4dd0-91ec-9792ebfc16b4'
       , N'Organization 13 plan'
       , 100
       , 100
       , 1000)

     , ( '8106d634-dc06-caef-71b8-fa087228ecec'
       , @CurrentDate
       , @CurrentDate
       , 'eeb21d90-aba6-91a7-baeb-246dac582460'
       , NULL
       , '3cdc168a-61b2-4f68-bcd0-83531f4d8fcb'
       , N'Organization 14 plan'
       , 100
       , 100
       , 1000)

     , ( 'ebd39dbe-4d9a-3f62-a218-4bd228d715ad'
       , @CurrentDate
       , @CurrentDate
       , 'e85a8879-3edf-62be-8c41-668eb95bb860'
       , NULL
       , '1e3abdb2-2163-46af-88b0-9797f74c80e5'
       , N'Organization 15 plan'
       , 100
       , 100
       , 1000)

     , ( '46d75c50-ed47-7821-0bf1-55f967a757ef'
       , @CurrentDate
       , @CurrentDate
       , '1a21a037-a1b7-db64-7a18-3e94f799b107'
       , NULL
       , '8d236877-c42d-4982-9716-238f9b45a57e'
       , N'Organization 16 plan'
       , 100
       , 100
       , 1000)

     , ( '51828ab0-5b32-4fb0-1367-c5ab224c2163'
       , @CurrentDate
       , @CurrentDate
       , '87972d96-c000-f3d9-735d-fa881854bd2f'
       , NULL
       , '07d4dd6c-c41a-4b01-b066-7beff9476f73'
       , N'Organization 17 plan'
       , 100
       , 100
       , 1000)

     , ( '95194075-197c-c239-e726-3c51d84de9db'
       , @CurrentDate
       , @CurrentDate
       , '6767b1b2-1adc-67d1-e3dd-72c99ec30260'
       , NULL
       , '63122cdc-3ff7-45c9-9c91-102d460506bd'
       , N'Organization 18 plan'
       , 100
       , 100
       , 1000)

     , ( '5b341028-28eb-99fc-9404-42840c440396'
       , @CurrentDate
       , @CurrentDate
       , '6577b06a-31d3-f449-e843-7cda34ae2e5a'
       , NULL
       , '7e0ed9fb-2a22-436b-a8b6-2c6de78246a3'
       , N'Organization 19 plan'
       , 100
       , 100
       , 1000)

     , ( '48e07a43-e8e2-0222-64b9-60506ebc9633'
       , @CurrentDate
       , @CurrentDate
       , 'a4f93cb0-5d60-b03a-3c8a-2608fbf4cafb'
       , NULL
       , 'e6ebfb64-6fda-4f21-8e19-caa6e4614b2b'
       , N'Organization 20 plan'
       , 100
       , 100
       , 1000);
GO
