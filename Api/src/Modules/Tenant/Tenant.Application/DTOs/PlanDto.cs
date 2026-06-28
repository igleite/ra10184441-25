using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record PlanDto : InactiveDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxUsers { get; set; } = 0;
    public int MaxClients { get; set; } = 0;
    public int MaxTickets { get; set; } = 0;
}

