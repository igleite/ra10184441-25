using BuildingBlocks.Application.Interfaces.Query;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.Chat;

public record GetChatByTicketIdQuery(Guid TicketId) : IQuery<ChatDto>;