using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class ArtifactMappings
{
    public static ArtifactDto ToDto(this Artifact entity)
    {
        return new ArtifactDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            RowVersion = entity.RowVersion,
            InactivedAt = entity.InactivedAt,
            ArtifactTypeId = entity.ArtifactTypeId,
            Name = entity.Name,
            Code = entity.Code
        };
    }
}
