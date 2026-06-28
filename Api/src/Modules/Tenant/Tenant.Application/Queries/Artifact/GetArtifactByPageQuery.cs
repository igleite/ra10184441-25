using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Artifact;

public record GetArtifactByPageQuery(Guid OrganizationId, Guid? ArtifactTypeId, int PageIndex, int PageSize) : IQuery<PageDto<ArtifactDto>>;
