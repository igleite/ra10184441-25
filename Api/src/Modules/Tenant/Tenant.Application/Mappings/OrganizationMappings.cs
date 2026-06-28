using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Mappings;

public static class OrganizationMappings
{
    public static OrganizationDto ToDto(this Organization entity)
    {
        var dto = new OrganizationDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.Name = entity.Name;
        dto.Document = entity.Document;
        dto.Slug = entity.Slug;

        return dto;
    }

    public static Organization ToEntity(this OrganizationDto dto)
    {
        var dateNow = dto.UpdatedAt;
        var entity = new Organization(dto.Id, dateNow);
        entity.SetName(dto.Name, dateNow);
        entity.SetDocument(dto.Document, dateNow);
        entity.SetSlug(dto.Slug, dateNow);

        return entity;
    }
}