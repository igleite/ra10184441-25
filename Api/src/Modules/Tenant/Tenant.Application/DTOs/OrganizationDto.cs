using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record OrganizationDto : EntityDto
{
    public string Name { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Slug {  get; set; } = string.Empty;
}