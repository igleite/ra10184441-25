using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface IOrganizationPlanRepository
{
    Task<OrganizationPlan?> GetByIdAsync(Guid id);
    Task<OrganizationPlan?> GetByOrganizationIdAsync(Guid organizationId);
    Task<bool> CreateAsync(OrganizationPlan organizationPlan);
    Task<bool> UpdateAsync(OrganizationPlan organizationPlan);
    Task<bool> DeleteAsync(OrganizationPlan organizationPlan);
}

