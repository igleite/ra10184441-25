using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface ICustomerUserRepository
{
    Task<CustomerUser?> GetByIdAsync(Guid organizationId, Guid id);
    Task<CustomerUser?> GetByUserIdAsync(Guid organizationId, Guid userId);
    Task<CustomerUser?> GetByCustomerIdAndUserIdAsync(Guid organizationId, Guid customerId, Guid userId);
    Task<bool> CreateAsync(CustomerUser customerUser);
    Task<bool> UpdateAsync(CustomerUser customerUser);
    Task<bool> DeleteAsync(CustomerUser customerUser);
}

