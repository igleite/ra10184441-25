using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using Identity.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Interfaces.Resolvers;
using Tenant.Domain.Entities;

namespace Api.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITenantResolver _tenantResolver;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IOrganizationUserRepository _organizationUserRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ICustomerUserRepository _customerUserRepository;

    public AuthService(
        IHttpContextAccessor httpContextAccessor,
        ITenantResolver tenantResolver,
        IOrganizationRepository organizationRepository,
        IOrganizationUserRepository organizationUserRepository,
        ITeamRepository teamRepository,
        ICustomerUserRepository customerUserRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _tenantResolver = tenantResolver;
        _organizationRepository = organizationRepository;
        _organizationUserRepository = organizationUserRepository;
        _teamRepository = teamRepository;
        _customerUserRepository = customerUserRepository;
    }

    public string? GetBearerToken()
    {
        var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authHeader) ||
            !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return null;

        return authHeader["Bearer ".Length..].Trim();
    }

    public async Task<IReadOnlyList<string>> ResolveRoleNamesAsync(User user, CancellationToken cancellationToken = default)
    {
        var roleIds = new List<Guid>();
        if (user.RoleId.HasValue)
            roleIds.Add(user.RoleId.Value);

        if (user.RoleId == RoleEnum.SuperAdmin.Id)
        {
            roleIds.Add(user.RoleId.Value);
            return ToRoleNames(roleIds);
        }

        if (user.RoleId == RoleEnum.Onboarding.Id)
        {
            roleIds.Add(user.RoleId.Value);
            return ToRoleNames(roleIds);
        }

        var tenant = _tenantResolver.ResolveWithSource();
        var slug = tenant.Slug?.Trim();

        if (string.IsNullOrWhiteSpace(slug))
        {
            roleIds.Add(RoleEnum.Nullable.Id);
            return ToRoleNames(roleIds);
        }

        var organization = await _organizationRepository.GetBySlugAsync(slug);
        if (organization is null)
            throw AppException.Unauthorized();

        var organizationUser = await _organizationUserRepository.GetByUserIdAsync(organization.Id, user.Id);
        var customerUser = await _customerUserRepository.GetByUserIdAsync(organization.Id, user.Id);

        if (organizationUser is null && customerUser is null)
            throw AppException.Unauthorized();

        if (organizationUser is not null)
        {
            var team = await _teamRepository.GetByIdAsync(organization.Id, organizationUser.TeamId);
            if (team is not null)
                roleIds.Add(team.RoleId);
        }

        if (customerUser is not null)
            roleIds.Add(customerUser.RoleId);

        return ToRoleNames(roleIds);
    }

    public async Task<IReadOnlyList<ClaimDto>> BuildExtraClaimsAsync(User user, CancellationToken cancellationToken = default)
    {
        var extraClaims = new List<ClaimDto>
        {
            ClaimDto.Create(ClaimTypeEnum.UserId, user.Id.ToString())
        };

        var tenant = _tenantResolver.ResolveWithSource();
        var slug = tenant.Slug?.Trim();

        if (!string.IsNullOrWhiteSpace(slug))
        {
            var organization = await _organizationRepository.GetBySlugAsync(slug);
            if (organization is not null)
            {
                extraClaims.Add(ClaimDto.Create(ClaimTypeEnum.OrganizationId, organization.Id.ToString()));
            }
        }

        return extraClaims;
    }

    public UserDto ToUserDto(User user, IReadOnlyList<string> roleNames) =>
        new()
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            UserName = user.Name,
            Claims = roleNames.Select(r => ClaimDto.Create(ClaimTypeEnum.Role, r)).ToList()
        };

    private static List<string> ToRoleNames(IEnumerable<Guid> roleIds) =>
        roleIds
            .Distinct()
            .Select(id => RoleBaseEnum.FindById(id)?.Role)
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role!)
            .Distinct()
            .ToList();
}
