namespace Identity.Application.DTOs;

public record LoginApiResponseDto
{
    public bool Success { get; init; }
    public LoginDataDto? Data { get; init; }
    public List<string>? Errors { get; init; }
}

