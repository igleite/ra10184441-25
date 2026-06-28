using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.ArtifactType;

public record GetArtifactTypeByPageQuery(Guid OrganizationId, int PageIndex, int PageSize) : IQuery<PageDto<ArtifactTypeDto>>;
