using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface ICustomerArtifactRepository
{
    Task<CustomerArtifact?> GetByIdAsync(Guid organizationId, Guid id);
    Task<CustomerArtifact?> GetByCustomerIdAndArtifactIdAsync(Guid organizationId, Guid customerId, Guid artifactId);
    Task<bool> CreateAsync(CustomerArtifact customerArtifact);
    Task<bool> UpdateAsync(CustomerArtifact customerArtifact);
    Task<bool> DeleteAsync(CustomerArtifact customerArtifact);
}
