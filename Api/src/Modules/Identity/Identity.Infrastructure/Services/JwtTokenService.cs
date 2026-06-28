using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuildingBlocks.Application.Enums;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _settings;

    public JwtTokenService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public (string Token, DateTime ExpiresAt) GenerateToken(
        string userId,
        string userName,
        string email,
        IReadOnlyList<string> roles,
        IReadOnlyList<ClaimDto>? extraClaims = null)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.Email, email),
            new(ClaimTypeEnum.UserId.Type, userId),
        };

        if (extraClaims is not null)
        {
            foreach (var extraClaim in extraClaims)
            {
                if (string.IsNullOrWhiteSpace(extraClaim.Type) || string.IsNullOrWhiteSpace(extraClaim.Value))
                    continue;

                claims.Add(new Claim(extraClaim.Type, extraClaim.Value));
            }
        }

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypeEnum.Role.Type, role));

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenString, expiresAt);
    }
}
