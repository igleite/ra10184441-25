using BuildingBlocks.Application.Enums;

namespace Identity.Application.DTOs;

public record ClaimDto
{
    public string Value { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;

    public static ClaimDto Create(ClaimTypeBaseEnum claimType, string value) =>
        new() { Type = claimType.Type, Value = value };
}

