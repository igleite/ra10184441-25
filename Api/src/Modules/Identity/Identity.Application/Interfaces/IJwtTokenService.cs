using Identity.Application.DTOs;

namespace Identity.Application.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(
        string userId,
        string userName,
        string email,
        IReadOnlyList<string> roles,
        IReadOnlyList<ClaimDto>? extraClaims = null);
}
