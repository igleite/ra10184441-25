using Tickets.Application.DTOs;
using Tickets.Domain.Entities;

namespace Tickets.Application.Mappings;

public static class TicketClassificationMappings
{
    public static TicketClassificationDto ToDto(this TicketClassification entity)
    {
        return new TicketClassificationDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            RowVersion = entity.RowVersion,
            InactivedAt = entity.InactivedAt,
            OrganizationId = entity.OrganizationId,
            Name = entity.Name,
            Code = entity.Code
        };
    }
}
