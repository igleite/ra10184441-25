using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;

namespace Tickets.Application.Commands.Tickets;

public record DeleteTicketCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<TicketDto>;
