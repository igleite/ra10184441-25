using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class CustomerUserMappings
{
    public static CustomerUserDto ToDto(this CustomerUser entity)
    {
        var dto = new CustomerUserDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.OrganizationId = entity.OrganizationId;
        dto.InactivedAt = entity.InactivedAt;
        dto.CustomerId = entity.CustomerId;
        dto.UserId = entity.UserId;
        dto.RoleId = entity.RoleId;

        return dto;
    }

    public static CustomerUser ToEntity(this CustomerUserDto dto)
    {
        var dateNow = dto.UpdatedAt;
        var entity = new CustomerUser(dto.Id, dto.CreatedAt, dto.OrganizationId);
        entity.SetInactivedAt(dto.InactivedAt, dateNow);
        entity.SetCustomerId(dto.CustomerId, dateNow);
        entity.SetUserId(dto.UserId, dateNow);
        entity.SetRoleId(dto.RoleId, dateNow);

        return entity;
    }
}

