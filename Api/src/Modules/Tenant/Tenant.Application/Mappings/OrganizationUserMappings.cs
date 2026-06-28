using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class OrganizationUserMappings
{
    public static OrganizationUserDto ToDto(this OrganizationUser entity)
    {
        var dto = new OrganizationUserDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.OrganizationId = entity.OrganizationId;
        dto.InactivedAt = entity.InactivedAt;
        dto.UserId = entity.UserId;
        dto.TeamId = entity.TeamId;

        return dto;
    }

    public static OrganizationUser ToEntity(this OrganizationUserDto dto)
    {
        var dateNow = dto.UpdatedAt;
        var entity = new OrganizationUser(dto.Id, dto.CreatedAt, dto.OrganizationId);
        entity.SetInactivedAt(dto.InactivedAt, dateNow);
        entity.SetUserId(dto.UserId, dateNow);
        entity.SetTeamId(dto.TeamId, dateNow);

        return entity;
    }
}

