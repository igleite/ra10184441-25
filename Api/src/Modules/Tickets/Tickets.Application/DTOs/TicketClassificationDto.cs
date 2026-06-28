using BuildingBlocks.Application.DTOs;

namespace Tickets.Application.DTOs;

public record TicketClassificationDto : OrganizationInactiveDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
