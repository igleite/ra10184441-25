namespace BuildingBlocks.Application.DTOs;

public record OrganizationInactiveDto : EntityDto
{
    public Guid OrganizationId { get; set; }
    public DateTime? InactivedAt { get; set; }
}

