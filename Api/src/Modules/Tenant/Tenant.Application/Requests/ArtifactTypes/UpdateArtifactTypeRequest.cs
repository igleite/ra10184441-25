namespace Tenant.Application.Requests.ArtifactTypes;

public record UpdateArtifactTypeRequest(string Name, byte[] RowVersion);
