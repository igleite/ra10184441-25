using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class UserMappings
{
    public static UserDto ToDto(this User entity)
    {
        var dto = new UserDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.InactivedAt = entity.InactivedAt;
        dto.Name = entity.Name;
        dto.Email = entity.Email;
        dto.RoleId = entity.RoleId;

        return dto;
    }

    public static User ToEntity(this UserDto dto)
    {
        var dateNow = dto.UpdatedAt;
        var entity = new User(dto.Id, dto.CreatedAt);
        entity.SetInactivedAt(dto.InactivedAt, dateNow);
        entity.SetName(dto.Name, dateNow);
        entity.SetEmail(dto.Email, dateNow);
        entity.SetRoleId(dto.RoleId, dateNow);

        return entity;
    }
}

