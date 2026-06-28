using BuildingBlocks.Application.DTOs;
using Tickets.Domain.ValueObjects;

namespace Tickets.Application.DTOs;

public record StatusReasonDto : OrganizationInactiveDto
{
    public StatusType Type { get; set; } = StatusType.Open;
    public string Name { get; set; } = String.Empty;
    public bool IsOpeningDefault { get; set; } = false;
}

