using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;

namespace Tickets.Application.Commands.Tickets;

public record UpdateTicketCommand(Guid OrganizationId, Guid Id, Guid StatusId, Guid ClassificationId, Guid ArtifactId, int AllocationCenter, string Description, string Resolution, byte[] RowVersion) : ICommand<TicketDto>;
