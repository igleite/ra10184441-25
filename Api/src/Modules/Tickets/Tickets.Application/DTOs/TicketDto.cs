using BuildingBlocks.Application.DTOs;

namespace Tickets.Application.DTOs;

public record TicketDto : OrganizationSoftDeleteDto
{
    public Guid CustomerId { get; set; } = Guid.Empty;
    public Guid ArtifactId { get; set; } = Guid.Empty;
    public Guid StatusId { get; set; } = Guid.Empty;
    public Guid ClassificationId { get; set; } = Guid.Empty;
    public int AllocationCenter { get; set; }
    public Guid CreatedByUserId { get; set; } = Guid.Empty;
    public string Description { get; set; } = String.Empty;
    public string Resolution { get; set; } = String.Empty;
}