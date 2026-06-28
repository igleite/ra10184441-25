using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Artifact;

public record GetArtifactByIdQuery(Guid OrganizationId, Guid Id) : IQuery<ArtifactDto>;
