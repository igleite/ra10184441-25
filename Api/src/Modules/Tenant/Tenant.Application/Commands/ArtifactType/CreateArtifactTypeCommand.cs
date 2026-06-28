using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.ArtifactType;

public record CreateArtifactTypeCommand(Guid OrganizationId, string Name) : ICommand<ArtifactTypeDto>;
