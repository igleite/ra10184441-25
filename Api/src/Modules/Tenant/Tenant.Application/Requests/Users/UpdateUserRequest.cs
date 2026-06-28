using BuildingBlocks.Application.Enums;

namespace Tenant.Application.Requests.Users;

public record UpdateUserRequest(string Name, string Email, byte[] RowVersion, RoleBaseEnum? Role = null);