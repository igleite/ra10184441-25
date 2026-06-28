using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class PlanMappings
{
    public static PlanDto ToDto(this Plan entity)
    {
        var dto = new PlanDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.InactivedAt = entity.InactivedAt;
        dto.Name = entity.Name;
        dto.Description = entity.Description;
        dto.MaxUsers = entity.MaxUsers;
        dto.MaxClients = entity.MaxClients;
        dto.MaxTickets = entity.MaxTickets;

        return dto;
    }

    public static Plan ToEntity(this PlanDto dto)
    {
        var dateNow = dto.UpdatedAt;
        var entity = new Plan(dto.Id, dateNow);
        entity.SetInactivedAt(dto.InactivedAt, dateNow);
        entity.SetName(dto.Name, dateNow);
        entity.SetDescription(dto.Description, dateNow);
        entity.SetMaxUsers(dto.MaxUsers, dateNow);
        entity.SetMaxClients(dto.MaxClients, dateNow);
        entity.SetMaxTickets(dto.MaxTickets, dateNow);

        return entity;
    }
}

