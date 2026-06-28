namespace Tenant.Application.Requests.Organizations;

public record CreateOrganizationRequest(string Name, string Document, string Slug);