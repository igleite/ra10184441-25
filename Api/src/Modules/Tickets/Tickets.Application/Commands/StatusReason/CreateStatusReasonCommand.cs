using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;
using Tickets.Domain.ValueObjects;

namespace Tickets.Application.Commands.StatusReason;

public record CreateStatusReasonCommand(Guid OrganizationId, StatusType Type, string Name, bool IsOpeningDefault = false) : ICommand<StatusReasonDto>;

