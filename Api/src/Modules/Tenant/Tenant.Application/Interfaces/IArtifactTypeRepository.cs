using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface IArtifactTypeRepository
{
    Task<ArtifactType?> GetByIdAsync(Guid organizationId, Guid id);
    Task<ArtifactType?> GetByNameAsync(Guid organizationId, string name);
    Task<bool> CreateAsync(ArtifactType artifactType);
    Task<bool> UpdateAsync(ArtifactType artifactType);
    Task<bool> DeleteAsync(ArtifactType artifactType);
}
