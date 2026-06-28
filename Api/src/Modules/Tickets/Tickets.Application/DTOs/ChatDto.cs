using BuildingBlocks.Application.DTOs;

namespace Tickets.Application.DTOs;

public record ChatDto : SoftDeleteDto
{
    public Guid TicketId { get; set; } = Guid.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    public string Message { get; set; } = string.Empty;
}