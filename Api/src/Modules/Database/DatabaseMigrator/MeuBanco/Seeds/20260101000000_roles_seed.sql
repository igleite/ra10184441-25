DECLARE @CurrentDate DATETIME2 = GETUTCDATE();

DECLARE @SuperAdminRoleId UNIQUEIDENTIFIER = '8f42805c-d971-4fdb-b099-d69c6dc6b2a4';
DECLARE @OnboardingRoleId UNIQUEIDENTIFIER = '34de239d-221a-4345-97ac-80e9fcfa5b85';
DECLARE @OrganizationOwnerRoleId UNIQUEIDENTIFIER = 'e597e22c-e22e-4849-928c-e919717d2b8c';
DECLARE @OrganizationMemberRoleId UNIQUEIDENTIFIER = 'dc8c024b-69e2-4800-af50-f0fbd690279d';
DECLARE @ClientAdminRoleId UNIQUEIDENTIFIER = '7d99c5e6-e0ca-4189-b9aa-fb2c8c5ba0f5';
DECLARE @ClientMemberRoleId UNIQUEIDENTIFIER = '11fe3aaa-d36d-4cae-9bce-7c5084360425';

INSERT INTO [tenants].[role] ( [id]
                             , [name]
                             , [scope]
                             , [created_at]
                             , [updated_at])
VALUES ( @SuperAdminRoleId
       , N'SuperAdmin'
       , N'global'
       , @CurrentDate
       , @CurrentDate)

     , ( @OnboardingRoleId
       , N'Onboarding'
       , N'global'
       , @CurrentDate
       , @CurrentDate)

     , ( @OrganizationOwnerRoleId
       , N'OrganizationOwner'
       , N'organization'
       , @CurrentDate
       , @CurrentDate)

     , ( @OrganizationMemberRoleId
       , N'OrganizationMember'
       , N'organization'
       , @CurrentDate
       , @CurrentDate)

     , ( @ClientAdminRoleId
       , N'ClientAdmin'
       , N'customer'
       , @CurrentDate
       , @CurrentDate)

     , ( @ClientMemberRoleId
       , N'ClientMember'
       , N'customer'
       , @CurrentDate
       , @CurrentDate);
GO
