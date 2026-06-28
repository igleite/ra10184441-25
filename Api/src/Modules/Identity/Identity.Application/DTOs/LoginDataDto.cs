namespace Identity.Application.DTOs;

public record LoginDataDto
{
    public string Token { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime ExpiresIn { get; init; }
    public UserDto User { get; init; } = new();
}

