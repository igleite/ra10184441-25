using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.CustomerArtifact;

public record DeleteCustomerArtifactCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<CustomerArtifactDto>;
