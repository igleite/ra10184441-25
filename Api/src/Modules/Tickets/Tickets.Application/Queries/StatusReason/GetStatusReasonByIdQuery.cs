using BuildingBlocks.Application.Interfaces.Query;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.StatusReason;

public record GetStatusReasonByIdQuery(Guid OrganizationId, Guid Id) : IQuery<StatusReasonDto>;

