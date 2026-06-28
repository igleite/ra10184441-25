using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.ArtifactType;

public record DeleteArtifactTypeCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<ArtifactTypeDto>;
