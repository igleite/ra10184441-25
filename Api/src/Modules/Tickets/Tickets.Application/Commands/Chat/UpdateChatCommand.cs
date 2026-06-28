using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;

namespace Tickets.Application.Commands.Chat;

public record UpdateChatCommand(Guid OrganizationId, Guid Id, Guid TicketId, string Message, byte[] RowVersion) : ICommand<ChatDto>;

