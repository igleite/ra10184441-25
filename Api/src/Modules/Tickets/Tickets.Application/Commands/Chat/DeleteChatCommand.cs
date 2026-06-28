using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;

namespace Tickets.Application.Commands.Chat;

public record DeleteChatCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<ChatDto>;

