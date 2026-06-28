using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.CustomerArtifact;

public record GetCustomerArtifactByIdQuery(Guid OrganizationId, Guid Id) : IQuery<CustomerArtifactDto>;
