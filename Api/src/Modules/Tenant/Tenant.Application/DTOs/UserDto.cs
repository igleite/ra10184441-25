using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record UserDto : InactiveDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid? RoleId { get; set; }
}

