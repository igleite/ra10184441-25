namespace Tenant.Application.Requests.Teams;

public record UpdateTeamRequest(string Name, string Code, Guid RoleId, byte[] RowVersion);
