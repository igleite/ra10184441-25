namespace Tenant.Application.Requests.OrganizationUsers;

public record CreateOrganizationUserRequest(Guid UserId, Guid TeamId);