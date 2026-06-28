using Tickets.Domain.Entities;

namespace Tickets.Application.Interfaces;

public interface IStatusReasonRepository
{
    Task<StatusReason?> GetByIdAsync(Guid organizationId, Guid id);
    Task<StatusReason?> GetByNameAsync(Guid organizationId, string name);
    Task<StatusReason?> GetByOpeningDefaultAsync(Guid organizationId);
    Task<bool> CreateAsync(StatusReason statusReason);
    Task<bool> UpdateAsync(StatusReason statusReason);
    Task<bool> DeleteAsync(StatusReason statusReason);
}

