using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class OrganizationPlanMappings
{
    public static OrganizationPlanDto ToDto(this OrganizationPlan entity)
    {
        var dto = new OrganizationPlanDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.InactivedAt = entity.InactivedAt;
        dto.OrganizationId = entity.OrganizationId;
        dto.PlanId = entity.PlanId;
        dto.Description = entity.Description;
        dto.MaxUsers = entity.MaxUsers;
        dto.MaxClients = entity.MaxClients;
        dto.MaxTickets = entity.MaxTickets;

        return dto;
    }

    public static OrganizationPlan ToEntity(this OrganizationPlanDto dto)
    {
        var dateNow = dto.UpdatedAt;
        var entity = new OrganizationPlan(dto.Id, dateNow, dto.OrganizationId);
        entity.SetInactivedAt(dto.InactivedAt, dateNow);
        entity.SetPlanId(dto.PlanId, dateNow);
        entity.SetDescription(dto.Description, dateNow);
        entity.SetMaxUsers(dto.MaxUsers, dateNow);
        entity.SetMaxClients(dto.MaxClients, dateNow);
        entity.SetMaxTickets(dto.MaxTickets, dateNow);

        return entity;
    }
}

