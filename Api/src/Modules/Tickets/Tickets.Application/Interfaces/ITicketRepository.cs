using Tickets.Domain.Entities;

namespace Tickets.Application.Interfaces;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(Guid organizationId, Guid id);
    Task<bool> CreateAsync(Ticket ticket);
    Task<bool> UpdateAsync(Ticket ticket);
    Task<bool> DeleteAsync(Ticket ticket);
}

