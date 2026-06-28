namespace Identity.Application.DTOs;

public record UserDto
{
    public string Id { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public List<ClaimDto> Claims { get; init; } = new();
}

