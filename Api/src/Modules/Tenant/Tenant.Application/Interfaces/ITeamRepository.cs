using Tenant.Domain.Entities;

namespace Tenant.Application.Interfaces;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid organizationId, Guid id);
    Task<Team?> GetByCodeAsync(Guid organizationId, string code);
    Task<Team?> GetByNameAsync(Guid organizationId, string name);
    Task<bool> CreateAsync(Team team);
    Task<bool> UpdateAsync(Team team);
    Task<bool> DeleteAsync(Team team);
    Task<int> CountActiveByOrganizationIdAsync(Guid organizationId);
}
