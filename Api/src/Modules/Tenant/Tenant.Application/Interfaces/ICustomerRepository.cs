using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid organizationId, Guid id);
    Task<Customer?> GetByDocumentAsync(Guid organizationId, string document);
    Task<bool> CreateAsync(Customer customer);
    Task<bool> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(Customer customer);
}