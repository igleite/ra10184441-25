using Tickets.Application.DTOs;
using Tickets.Domain.Entities;
using Tickets.Domain.ValueObjects;

namespace Tickets.Application.Mappings;

public static class TicketMappings
{
    public static TicketDto ToDto(this Ticket entity)
    {
        var dto = new TicketDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.OrganizationId = entity.OrganizationId;
        dto.DeletedAt = entity.DeletedAt;
        dto.CustomerId = entity.CustomerId;
        dto.ArtifactId = entity.ArtifactId;
        dto.StatusId = entity.StatusId;
        dto.ClassificationId = entity.ClassificationId;
        dto.AllocationCenter = entity.AllocationCenter.Value;
        dto.CreatedByUserId = entity.CreatedByUserId;
        dto.Description = entity.Description;
        dto.Resolution = entity.Resolution;

        return dto;
    }

    public static Ticket ToEntity(this TicketDto dto)
    {
        var dateNow = dto.UpdatedAt;
        var entity = new Ticket(dto.Id, dto.CreatedAt, dto.OrganizationId);
        entity.SetDeletedAt(dto.DeletedAt, dateNow);
        entity.SetCustomerId(dto.CustomerId, dateNow);
        entity.SetArtifactId(dto.ArtifactId, dateNow);
        entity.SetStatusId(dto.StatusId, dateNow);
        entity.SetClassificationId(dto.ClassificationId, dateNow);
        entity.SetAllocationCenter(AllocationCenter.From(dto.AllocationCenter), dateNow);
        entity.SetCreatedByUserId(dto.CreatedByUserId, dateNow);
        entity.SetDescription(dto.Description, dateNow);
        entity.SetResolution(dto.Resolution, dateNow);

        return entity;
    }
}
