namespace BuildingBlocks.Application.DTOs;

public record InactiveDto : EntityDto
{
    public DateTime? InactivedAt { get; set; }
}

