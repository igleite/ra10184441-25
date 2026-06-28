using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Artifact;

public record UpdateArtifactCommand(Guid OrganizationId, Guid Id, Guid ArtifactTypeId, string Name, string Code, byte[] RowVersion) : ICommand<ArtifactDto>;
