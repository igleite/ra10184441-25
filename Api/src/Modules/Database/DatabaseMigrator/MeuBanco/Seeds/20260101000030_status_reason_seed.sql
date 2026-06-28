DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

DECLARE @OrganizationId UNIQUEIDENTIFIER = '45143ad7-dc93-4664-982e-b93c7feb5f75';

INSERT INTO [tickets].[status_reason] ( [id]
                                      , [created_at]
                                      , [updated_at]
                                      , [organization_id]
                                      , [inactived_at]
                                      , [type]
                                      , [name]
                                      , [is_opening_default])
VALUES ( '9b636342-19a7-680c-cc2a-20520ccaec1b'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 1
       , N'Aberto'
       , 1)

     , ( 'de1c99a0-d9ab-2d71-2002-51a6634882f1'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 1
       , N'Em execuÃ§Ã£o'
       , 0)

     , ( 'a11a2f62-e77a-12ff-0802-79a3a6eca230'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 1
       , N'Aguardando cliente'
       , 0)

     , ( '7873dbd8-729e-8f8e-593d-84c5a026d7a1'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 1
       , N'Em anÃ¡lise'
       , 0)

     , ( 'dcfcfe82-3865-b003-7b1e-c9107d304297'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 1
       , N'Pausado'
       , 0)

     , ( 'c56341ed-7f75-41ed-4541-3f0c4307c4f6'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 1
       , N'Fechado'
       , 0)

     , ( '7ed42ede-8898-a688-5a68-f8680f0b40c6'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 1
       , N'Cancelado'
       , 0)

     , ( '9f2b3697-db48-a017-dd65-d6c806a9ed88'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 1
       , N'Resolvido'
       , 0)

     , ( '13ed6d91-f8f1-6161-14a4-7b438720a011'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 1
       , N'Reaberto'
       , 0)

     , ( 'd462ce9c-af27-e407-7a37-be99a6d72269'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 1
       , N'Escalado'
       , 0)

     , ( '6637848d-f887-f4f4-3f74-fd9200dfff47'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 2
       , N'Aberto 11'
       , 0)

     , ( '751decbb-dacc-7dde-0bd6-41e762fa23e5'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 2
       , N'Em execuÃ§Ã£o 12'
       , 0)

     , ( '4a50cbf3-a6c9-c471-b053-848dd0ba89e6'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 2
       , N'Aguardando cliente 13'
       , 0)

     , ( 'f1446da2-98ca-6506-0aaa-d0d4e7d5db4b'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 2
       , N'Em anÃ¡lise 14'
       , 0)

     , ( 'b43cf2cb-5afb-0362-0c91-3bff4547c428'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 2
       , N'Pausado 15'
       , 0)

     , ( 'f05051ef-c675-b069-1a88-63e067bbf47b'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 2
       , N'Fechado 16'
       , 0)

     , ( 'b3534f8d-533a-9b99-8e52-5c9366c6a66f'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 2
       , N'Cancelado 17'
       , 0)

     , ( 'f8674b5e-a695-885c-01e8-b564bc361384'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 2
       , N'Resolvido 18'
       , 0)

     , ( '92cafa7b-ddc0-933e-78bd-9eee9a2b6fe6'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 2
       , N'Reaberto 19'
       , 0)

     , ( 'ef2084c4-d6f0-b534-3d3e-4c67df63f5b5'
       , @CurrentDate
       , @CurrentDate
       , @OrganizationId
       , NULL
       , 2
       , N'Escalado 20'
       , 0);
GO
