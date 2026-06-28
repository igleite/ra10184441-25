namespace Tenant.Application.Requests.Teams;

public record CreateTeamRequest(string Name, string Code, Guid RoleId);
