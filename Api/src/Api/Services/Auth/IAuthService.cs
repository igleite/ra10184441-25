using Identity.Application.DTOs;
using Tenant.Domain.Entities;

namespace Api.Services.Auth;

public interface IAuthService
{
    string? GetBearerToken();
    Task<IReadOnlyList<string>> ResolveRoleNamesAsync(User user, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ClaimDto>> BuildExtraClaimsAsync(User user, CancellationToken cancellationToken = default);
    UserDto ToUserDto(User user, IReadOnlyList<string> roleNames);
}
