namespace BuildingBlocks.Application.DTOs;

public record OrganizationSoftDeleteDto : EntityDto
{
    public Guid OrganizationId { get; set; }
    public DateTime? DeletedAt { get; set; }
}
