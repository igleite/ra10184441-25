using BuildingBlocks.Application.Interfaces.Query;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.Tickets;

public record GetTicketByIdQuery(Guid OrganizationId, Guid Id) : IQuery<TicketDto>;