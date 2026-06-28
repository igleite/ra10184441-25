using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.ArtifactType;

public record UpdateArtifactTypeCommand(Guid OrganizationId, Guid Id, string Name, byte[] RowVersion) : ICommand<ArtifactTypeDto>;
