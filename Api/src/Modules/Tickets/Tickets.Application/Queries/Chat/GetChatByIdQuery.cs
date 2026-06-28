using BuildingBlocks.Application.Interfaces.Query;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.Chat;

public record GetChatByIdQuery(Guid OrganizationId, Guid TicketId, Guid Id) : IQuery<ChatDto>;