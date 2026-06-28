using Tickets.Domain.Entities;

namespace Tickets.Application.Interfaces;

public interface IChatRepository
{
    Task<Chat?> GetByIdAsync(Guid organizationId, Guid id);
    Task<Chat?> GetByTicketIdAsync(Guid ticketId);
    Task<bool> CreateAsync(Chat chat);
    Task<bool> UpdateAsync(Chat chat);
    Task<bool> DeleteAsync(Chat chat);
}