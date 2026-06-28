using BuildingBlocks.Application.Queries;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.Tickets;

public record GetTicketByPageQuery(Guid OrganizationId, int PageIndex, int PageSize) : PageQuery<TicketDto>(PageIndex, PageSize);
