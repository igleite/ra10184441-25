using FeatureFlags.Application.DTOs;
using FeatureFlags.Domain.Entities;

namespace FeatureFlags.Application.Mappings;

public static class FeatureFlagMappings
{
    public static FeatureFlagDto ToDto(this FeatureFlag entity)
    {
        var dto = new FeatureFlagDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.Name = entity.Name;
        dto.Description = entity.Description;
        dto.Value = entity.Value;

        return dto;
    }

    public static FeatureFlag ToEntity(this FeatureFlagDto dto)
    {
        var entity = new FeatureFlag(dto.Id, dto.CreatedAt);
        var dateNow = dto.UpdatedAt;
        entity.SetName(dto.Name, dateNow);
        entity.SetDescription(dto.Description, dateNow);
        entity.SetValue(dto.Value, dateNow);

        return entity;
    }
}


