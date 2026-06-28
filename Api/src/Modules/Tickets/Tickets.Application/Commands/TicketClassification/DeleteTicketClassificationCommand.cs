using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;

namespace Tickets.Application.Commands.TicketClassification;

public record DeleteTicketClassificationCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<TicketClassificationDto>;
