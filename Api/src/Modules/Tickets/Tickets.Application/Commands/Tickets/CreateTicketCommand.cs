using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;

namespace Tickets.Application.Commands.Tickets;

public record CreateTicketCommand(Guid OrganizationId, Guid CustomerId, Guid ArtifactId, Guid ClassificationId, Guid CreatedByUserId, string Description) : ICommand<TicketDto>;
