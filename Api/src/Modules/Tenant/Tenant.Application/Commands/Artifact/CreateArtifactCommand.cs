using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Artifact;

public record CreateArtifactCommand(Guid OrganizationId, Guid ArtifactTypeId, string Name, string Code) : ICommand<ArtifactDto>;
