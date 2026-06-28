using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.ArtifactType;

public record GetArtifactTypeByIdQuery(Guid OrganizationId, Guid Id) : IQuery<ArtifactTypeDto>;
