using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record OrganizationPlanDto : OrganizationInactiveDto
{
    public Guid PlanId { get; set; } = Guid.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxUsers { get; set; } = 0;
    public int MaxClients { get; set; } = 0;
    public int MaxTickets { get; set; } = 0;
}

