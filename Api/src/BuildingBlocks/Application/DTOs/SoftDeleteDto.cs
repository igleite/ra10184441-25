namespace BuildingBlocks.Application.DTOs;

public record SoftDeleteDto : EntityDto
{
    public DateTime? DeletedAt { get; set; }
}
