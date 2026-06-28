using BuildingBlocks.Application.Queries;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.StatusReason;

public record GetStatusReasonByPageQuery(Guid OrganizationId, int PageIndex, int PageSize) : PageQuery<StatusReasonDto>(PageIndex, PageSize);

