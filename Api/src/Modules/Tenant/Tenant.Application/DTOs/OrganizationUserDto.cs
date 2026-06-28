using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record OrganizationUserDto : OrganizationInactiveDto
{
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid TeamId { get; set; } = Guid.Empty;
}

