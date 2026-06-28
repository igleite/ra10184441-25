using BuildingBlocks.Application.Interfaces.Query;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.TicketClassification;

public record GetTicketClassificationByIdQuery(Guid OrganizationId, Guid Id) : IQuery<TicketClassificationDto>;
