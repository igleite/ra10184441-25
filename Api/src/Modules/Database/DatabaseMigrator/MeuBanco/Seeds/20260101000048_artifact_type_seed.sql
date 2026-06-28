DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

DECLARE @OrganizationId UNIQUEIDENTIFIER = '45143ad7-dc93-4664-982e-b93c7feb5f75';

INSERT INTO [tenants].[artifact_type] ( [id]
                                      , [created_at]
                                      , [updated_at]
                                      , [organization_id]
                                      , [inactived_at]
                                      , [name])
VALUES ( '4905f835-06f9-a62d-9feb-e6343939739e'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 1')

     , ( '79cd5abc-e529-e1a1-d9fc-3fff955fe83c'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 2')

     , ( 'dfd056de-7185-ff5d-624e-461fba7c3480'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 3')

     , ( 'd615baa4-7dc7-9ddc-dc9e-586900aea502'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 4')

     , ( '2017fc6e-7ed5-07d0-a948-b8adb3f3f7aa'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 5')

     , ( '81dadc3b-1620-2f50-787e-f7eee5546dbf'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 6')

     , ( '76f8ee3a-1f4e-bb8f-4e65-505a9fd7729e'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 7')

     , ( 'c72e63d4-4e32-df45-90e8-6052613e0256'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 8')

     , ( 'aa8ebed6-2602-ee98-7fb8-447226912df6'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 9')

     , ( '8c995ce3-1926-9288-29c9-9cc820c85c18'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 10')

     , ( '2e5fdeb0-2779-d063-8adf-f5667be8dd3b'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 11')

     , ( 'ed0f3912-2d07-4a47-ca5f-dfebdd9538ab'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 12')

     , ( 'dd55e2df-6912-a635-2753-5aced42ca3ff'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 13')

     , ( '22c95c02-3a35-82c3-e901-69e1113cd65f'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 14')

     , ( 'edf8b82d-eb7e-33ac-7298-17396c62448c'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 15')

     , ( 'cee99ea6-2b82-9192-8879-6810873f24ed'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 16')

     , ( '4856df3a-e155-2da8-7775-5abc174f4c6b'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 17')

     , ( '75f58ef2-fecc-5d49-8b8b-6490a22a4d93'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 18')

     , ( '3ff15ea1-a1e9-622b-ff59-cb8c3cc99ca6'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 19')

     , ( 'a32a600c-7f7b-d113-8867-15e807252f9c'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , N'Artifact Type 20');
GO
