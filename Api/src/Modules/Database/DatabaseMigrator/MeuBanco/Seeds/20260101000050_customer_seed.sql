DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

DECLARE @OrganizationId UNIQUEIDENTIFIER = '45143ad7-dc93-4664-982e-b93c7feb5f75';

INSERT INTO [tenants].[customer] ( [id]
                                 , [created_at]
                                 , [updated_at]
                                 , [organization_id]
                                 , [inactived_at]
                                 , [name]
                                 , [document])
VALUES ( '9d673361-ea55-292c-89f9-b4f94b676aca'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 1'
       , N'01.591.503/0001-01')

     , ( '143c7bfd-ac9e-e560-0886-7c864052a22d'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 2'
       , N'02.591.503/0001-02')

     , ( 'ef5ea879-764c-0bcc-a478-4ddbeceac20f'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 3'
       , N'03.591.503/0001-03')

     , ( 'ea0ead6d-1b7d-1ca9-092b-8193dae74490'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 4'
       , N'04.591.503/0001-04')

     , ( '15c041f1-3cd4-7d83-9d7c-52861ddea0dc'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 5'
       , N'05.591.503/0001-05')

     , ( '8d23ef19-7347-329b-28b5-e4c76592e312'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 6'
       , N'06.591.503/0001-06')

     , ( 'b55cad2e-4f3f-40a2-a027-370e347b471d'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 7'
       , N'07.591.503/0001-07')

     , ( 'd996f279-074b-a62f-2522-0707c6edadb4'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 8'
       , N'08.591.503/0001-08')

     , ( '2eda5200-62d5-4faa-760c-42e6bb4508ff'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 9'
       , N'09.591.503/0001-09')

     , ( 'ef70def3-c3e9-1ffa-1c02-9656422c6094'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 10'
       , N'10.591.503/0001-10')

     , ( '7d4de21e-9bd3-e168-01ff-c55681d5ca81'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 11'
       , N'11.591.503/0001-11')

     , ( '9e6c1e40-7ea1-ab06-f893-d3b76c8a9f78'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 12'
       , N'12.591.503/0001-12')

     , ( 'da5cd6ee-a072-722d-88a9-e5d1d37fcb08'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 13'
       , N'13.591.503/0001-13')

     , ( '1d38ccd0-095f-dc2a-54cc-731bc13b72f8'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 14'
       , N'14.591.503/0001-14')

     , ( '16c1c8ce-6c07-2dfc-0606-879ed9f19f09'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 15'
       , N'15.591.503/0001-15')

     , ( '292490c3-8064-677b-0eaa-ac803ead2a3d'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 16'
       , N'16.591.503/0001-16')

     , ( '00c32237-56f3-5185-61c9-95b7fe92f4c8'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 17'
       , N'17.591.503/0001-17')

     , ( 'de355b11-7d2a-d2c7-eb52-c3896fe13a2d'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 18'
       , N'18.591.503/0001-18')

     , ( '88c752a4-c238-787d-c781-4c15a4e4a190'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 19'
       , N'19.591.503/0001-19')

     , ( '569ef6ef-16cf-aa2a-f78a-6620f903abab'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Customer 20'
       , N'20.591.503/0001-20');
GO
