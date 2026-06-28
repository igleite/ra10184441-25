namespace Tenant.Application.Requests.OrganizationUsers;

public record UpdateOrganizationUserRequest(Guid UserId, Guid TeamId, byte[] RowVersion);