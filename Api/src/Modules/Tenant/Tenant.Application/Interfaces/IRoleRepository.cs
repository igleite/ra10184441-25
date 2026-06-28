using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id);
}
