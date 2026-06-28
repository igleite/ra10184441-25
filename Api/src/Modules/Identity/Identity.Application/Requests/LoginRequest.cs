namespace Identity.Application.Requests;

public record LoginRequest(string Email, string? AppOrigin = null);
