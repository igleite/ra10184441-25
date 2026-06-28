using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;
using Tickets.Domain.ValueObjects;

namespace Tickets.Application.Commands.StatusReason;

public record UpdateStatusReasonCommand(Guid OrganizationId, Guid Id, StatusType Type, string Name, byte[] RowVersion) : ICommand<StatusReasonDto>;

