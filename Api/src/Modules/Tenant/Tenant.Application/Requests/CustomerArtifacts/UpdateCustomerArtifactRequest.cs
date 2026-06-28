namespace Tenant.Application.Requests.CustomerArtifacts;

public record UpdateCustomerArtifactRequest(Guid ArtifactId, byte[] RowVersion);
