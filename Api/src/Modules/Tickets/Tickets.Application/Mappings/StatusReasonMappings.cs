using Tickets.Application.DTOs;
using Tickets.Domain.Entities;

namespace Tickets.Application.Mappings;

public static class StatusReasonMappings
{
    public static StatusReasonDto ToDto(this StatusReason entity)
    {
        var dto = new StatusReasonDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.OrganizationId = entity.OrganizationId;
        dto.InactivedAt = entity.InactivedAt;
        dto.Type = entity.Type;
        dto.Name = entity.Name;
        dto.IsOpeningDefault = entity.IsOpeningDefault;

        return dto;
    }

    public static StatusReason ToEntity(this StatusReasonDto dto)
    {
        var dateNow = dto.UpdatedAt;
        var entity = new StatusReason(dto.Id, dto.CreatedAt, dto.OrganizationId);
        entity.SetInactivedAt(dto.InactivedAt, dateNow);
        entity.SetType(dto.Type, dateNow);
        entity.SetName(dto.Name, dateNow);
        entity.SetIsOpeningDefault(dto.IsOpeningDefault, dateNow);

        return entity;
    }
}

