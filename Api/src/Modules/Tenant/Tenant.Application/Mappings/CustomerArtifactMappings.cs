using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class CustomerArtifactMappings
{
    public static CustomerArtifactDto ToDto(this CustomerArtifact entity)
    {
        return new CustomerArtifactDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            RowVersion = entity.RowVersion,
            InactivedAt = entity.InactivedAt,
            CustomerId = entity.CustomerId,
            ArtifactId = entity.ArtifactId
        };
    }
}
