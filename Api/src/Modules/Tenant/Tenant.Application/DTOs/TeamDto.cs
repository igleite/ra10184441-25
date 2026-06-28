using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record TeamDto : OrganizationInactiveDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
}
