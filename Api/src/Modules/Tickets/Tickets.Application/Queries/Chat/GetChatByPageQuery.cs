using BuildingBlocks.Application.Queries;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.Chat;

public record GetChatByPageQuery(Guid OrganizationId, Guid TicketId, int PageIndex, int PageSize) : PageQuery<ChatDto>(PageIndex, PageSize);
