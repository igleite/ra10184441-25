using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.CustomerArtifact;

public record UpdateCustomerArtifactCommand(Guid OrganizationId, Guid Id, Guid CustomerId, Guid ArtifactId, byte[] RowVersion) : ICommand<CustomerArtifactDto>;
