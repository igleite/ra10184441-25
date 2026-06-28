using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Query;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.TicketClassification;

public record GetTicketClassificationByPageQuery(Guid OrganizationId, int PageIndex, int PageSize) : IQuery<PageDto<TicketClassificationDto>>;
