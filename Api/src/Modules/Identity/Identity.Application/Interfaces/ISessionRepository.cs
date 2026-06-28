using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface ISessionRepository
{
    Task<Session?> GetByTokenAsync(string token);
    Task<bool> CreateAsync(Session session);
    Task<bool> UpdateAsync(Session session);
    Task<bool> DeleteByTokenAsync(string token);
}
