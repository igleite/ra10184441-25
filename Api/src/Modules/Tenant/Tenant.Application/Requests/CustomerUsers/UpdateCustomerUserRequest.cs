namespace Tenant.Application.Requests.CustomerUsers;

public record UpdateCustomerUserRequest(Guid UserId, byte[] RowVersion);