namespace Tenant.Application.Requests.Artifacts;

public record UpdateArtifactRequest(Guid ArtifactTypeId, string Name, string Code, byte[] RowVersion);
