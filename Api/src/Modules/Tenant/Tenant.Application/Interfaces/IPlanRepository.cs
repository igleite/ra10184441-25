using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface IPlanRepository
{
    Task<Plan?> GetByIdAsync(Guid id);
    Task<Plan?> GetByNameAsync(string name);
    Task<bool> CreateAsync(Plan plan);
    Task<bool> UpdateAsync(Plan plan);
    Task<bool> DeleteAsync(Plan plan);
}

