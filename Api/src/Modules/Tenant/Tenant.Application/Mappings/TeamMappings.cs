using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class TeamMappings
{
    public static TeamDto ToDto(this Team entity)
    {
        return new TeamDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            RowVersion = entity.RowVersion,
            InactivedAt = entity.InactivedAt,
            OrganizationId = entity.OrganizationId,
            Name = entity.Name,
            Code = entity.Code,
            RoleId = entity.RoleId
        };
    }
}
