using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record CustomerUserDto : OrganizationInactiveDto
{
    public Guid CustomerId { get; set; } = Guid.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid RoleId { get; set; }
}

