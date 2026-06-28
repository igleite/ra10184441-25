using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;

namespace Tickets.Application.Commands.Chat;

public record CreateChatCommand(Guid OrganizationId, Guid TicketId, Guid UserId, string Message) : ICommand<ChatDto>;

