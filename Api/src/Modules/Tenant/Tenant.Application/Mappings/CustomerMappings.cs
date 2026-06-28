using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class CustomerMappings
{
    public static CustomerDto ToDto(this Customer entity)
    {
        var dto = new CustomerDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.InactivedAt = entity.InactivedAt;
        dto.OrganizationId = entity.OrganizationId;
        dto.Name = entity.Name;
        dto.Document = entity.Document;

        return dto;
    }

    public static Customer ToEntity(this CustomerDto dto)
    {
        var dateNow = dto.UpdatedAt;
        var entity = new Customer(dto.Id, dateNow, dto.OrganizationId);
        entity.SetInactivedAt(dto.InactivedAt, dateNow);  
        entity.SetName(dto.Name, dateNow);
        entity.SetDocument(dto.Document, dateNow);

        return entity;
    }
}