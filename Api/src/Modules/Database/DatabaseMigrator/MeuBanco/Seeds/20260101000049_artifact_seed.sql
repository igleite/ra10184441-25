DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

INSERT INTO [tenants].[artifact] ( [id]
                                 , [created_at]
                                 , [updated_at]
                                 , [inactived_at]
                                 , [artifact_type_id]
                                 , [name]
                                 , [code])
VALUES ( '4f6ac6cf-e672-8ed7-c00c-2715e75869d2'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '4905f835-06f9-a62d-9feb-e6343939739e'
       , N'Artifact 1'
       , N'ART001')

     , ( 'ceb87934-f577-0ba7-2421-b84c49efb9a3'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '79cd5abc-e529-e1a1-d9fc-3fff955fe83c'
       , N'Artifact 2'
       , N'ART002')

     , ( '9d8c1019-6db5-fc51-e5a7-50a2e2ba3325'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'dfd056de-7185-ff5d-624e-461fba7c3480'
       , N'Artifact 3'
       , N'ART003')

     , ( 'd7859390-3f6b-0714-c7d1-797d12e72909'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'd615baa4-7dc7-9ddc-dc9e-586900aea502'
       , N'Artifact 4'
       , N'ART004')

     , ( '489dc813-3e8f-e2be-7cd0-bb380cdce886'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '2017fc6e-7ed5-07d0-a948-b8adb3f3f7aa'
       , N'Artifact 5'
       , N'ART005')

     , ( 'b4635e55-fe9b-5ea7-7cc4-87cf9a5acd34'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '81dadc3b-1620-2f50-787e-f7eee5546dbf'
       , N'Artifact 6'
       , N'ART006')

     , ( '8c0382b1-cca6-4fb5-4385-8b702a751abd'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '76f8ee3a-1f4e-bb8f-4e65-505a9fd7729e'
       , N'Artifact 7'
       , N'ART007')

     , ( 'a9fde140-1c8a-cbd4-2bec-d37c3e6664b8'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'c72e63d4-4e32-df45-90e8-6052613e0256'
       , N'Artifact 8'
       , N'ART008')

     , ( 'de2e7751-c3b1-281a-15bc-f6ad17f044c6'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'aa8ebed6-2602-ee98-7fb8-447226912df6'
       , N'Artifact 9'
       , N'ART009')

     , ( '6ab72e9b-5dda-1709-fbef-7f2ab2bbff69'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '8c995ce3-1926-9288-29c9-9cc820c85c18'
       , N'Artifact 10'
       , N'ART010')

     , ( 'beb97496-3065-c282-f9a9-d9c0d7a1734a'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '2e5fdeb0-2779-d063-8adf-f5667be8dd3b'
       , N'Artifact 11'
       , N'ART011')

     , ( '1c2ff3f6-ec10-8260-a8e7-75b8e825f8aa'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'ed0f3912-2d07-4a47-ca5f-dfebdd9538ab'
       , N'Artifact 12'
       , N'ART012')

     , ( '2850ff70-75f9-5162-6a47-91e9692a3316'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'dd55e2df-6912-a635-2753-5aced42ca3ff'
       , N'Artifact 13'
       , N'ART013')

     , ( '6f58049e-4c26-7b26-808c-a59116b90331'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '22c95c02-3a35-82c3-e901-69e1113cd65f'
       , N'Artifact 14'
       , N'ART014')

     , ( 'f45dd17e-65c9-6896-867f-3f1931eeb15c'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'edf8b82d-eb7e-33ac-7298-17396c62448c'
       , N'Artifact 15'
       , N'ART015')

     , ( '32dd8e09-55cf-9d06-c522-cc4a7ac36120'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'cee99ea6-2b82-9192-8879-6810873f24ed'
       , N'Artifact 16'
       , N'ART016')

     , ( '677fddbe-ea69-c08d-f4d7-316cf4d6c724'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '4856df3a-e155-2da8-7775-5abc174f4c6b'
       , N'Artifact 17'
       , N'ART017')

     , ( '14f2a37c-8111-0463-12ca-f04b2c34ccae'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '75f58ef2-fecc-5d49-8b8b-6490a22a4d93'
       , N'Artifact 18'
       , N'ART018')

     , ( 'c2d2d9ae-163a-40d7-a4eb-be89894dd2e4'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , '3ff15ea1-a1e9-622b-ff59-cb8c3cc99ca6'
       , N'Artifact 19'
       , N'ART019')

     , ( 'fc6da8cd-fdff-773a-8c65-382a0925a92f'
       , @CurrentDate
       , @CurrentDate
       , NULL
       , 'a32a600c-7f7b-d113-8867-15e807252f9c'
       , N'Artifact 20'
       , N'ART020');
GO
