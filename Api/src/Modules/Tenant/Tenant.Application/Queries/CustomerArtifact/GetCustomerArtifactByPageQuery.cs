using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.CustomerArtifact;

public record GetCustomerArtifactByPageQuery(Guid OrganizationId, Guid? CustomerId, int PageIndex, int PageSize) : IQuery<PageDto<CustomerArtifactDto>>;
