using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;

namespace Tickets.Application.Commands.TicketClassification;

public record UpdateTicketClassificationCommand(Guid OrganizationId, Guid Id, string Name, string Code, byte[] RowVersion) : ICommand<TicketClassificationDto>;
