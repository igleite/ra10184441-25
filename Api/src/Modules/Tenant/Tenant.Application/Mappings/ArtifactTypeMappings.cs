using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class ArtifactTypeMappings
{
    public static ArtifactTypeDto ToDto(this ArtifactType entity)
    {
        return new ArtifactTypeDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            RowVersion = entity.RowVersion,
            InactivedAt = entity.InactivedAt,
            OrganizationId = entity.OrganizationId,
            Name = entity.Name
        };
    }
}
