using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface IArtifactRepository
{
    Task<Artifact?> GetByIdAsync(Guid organizationId, Guid id);
    Task<Artifact?> GetByArtifactTypeIdAndNameAsync(Guid organizationId, Guid artifactTypeId, string name);
    Task<Artifact?> GetByArtifactTypeIdAndCodeAsync(Guid organizationId, Guid artifactTypeId, string code);
    Task<bool> CreateAsync(Artifact artifact);
    Task<bool> UpdateAsync(Artifact artifact);
    Task<bool> DeleteAsync(Artifact artifact);
}
