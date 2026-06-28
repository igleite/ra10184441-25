using Tickets.Domain.Entities;

namespace Tickets.Application.Interfaces;

public interface ITicketClassificationRepository
{
    Task<TicketClassification?> GetByIdAsync(Guid organizationId, Guid id);
    Task<TicketClassification?> GetByCodeAsync(Guid organizationId, string code);
    Task<TicketClassification?> GetByNameAsync(Guid organizationId, string name);
    Task<bool> CreateAsync(TicketClassification ticketClassification);
    Task<bool> UpdateAsync(TicketClassification ticketClassification);
    Task<bool> DeleteAsync(TicketClassification ticketClassification);
}
