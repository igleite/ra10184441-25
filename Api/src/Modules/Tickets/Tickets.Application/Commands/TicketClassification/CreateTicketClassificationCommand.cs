using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;

namespace Tickets.Application.Commands.TicketClassification;

public record CreateTicketClassificationCommand(Guid OrganizationId, string Name, string Code) : ICommand<TicketClassificationDto>;
