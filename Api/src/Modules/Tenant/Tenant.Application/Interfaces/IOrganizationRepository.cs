using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface IOrganizationRepository
{
    Task<Organization?> GetByIdAsync(Guid id);
    Task<Organization?> GetBySlugAsync(string slug);
    Task<Organization?> GetByDocumentAsync(string document);
    Task<bool> CreateAsync(Organization organization);
    Task<bool> UpdateAsync(Organization organization);
    Task<bool> DeleteAsync(Organization organization);
}
