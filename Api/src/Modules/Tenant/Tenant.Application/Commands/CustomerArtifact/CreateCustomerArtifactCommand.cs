using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.CustomerArtifact;

public record CreateCustomerArtifactCommand(Guid OrganizationId, Guid CustomerId, Guid ArtifactId) : ICommand<CustomerArtifactDto>;
