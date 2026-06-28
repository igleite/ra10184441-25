using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Artifact;

public record DeleteArtifactCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<ArtifactDto>;
