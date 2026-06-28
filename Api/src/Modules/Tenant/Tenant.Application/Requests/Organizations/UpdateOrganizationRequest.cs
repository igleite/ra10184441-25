namespace Tenant.Application.Requests.Organizations;

public record UpdateOrganizationRequest(string Name, string Document, string Slug, byte[] RowVersion);