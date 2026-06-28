using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface IOrganizationUserRepository
{
    Task<OrganizationUser?> GetByIdAsync(Guid organizationId, Guid id);
    Task<OrganizationUser?> GetByUserIdAsync(Guid organizationId, Guid userId);
    Task<bool> CreateAsync(OrganizationUser organizationUser);
    Task<bool> UpdateAsync(OrganizationUser organizationUser);
    Task<bool> DeleteAsync(OrganizationUser organizationUser);
}

