using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;

namespace Tickets.Application.Commands.StatusReason;

public record DeleteStatusReasonCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<StatusReasonDto>;

