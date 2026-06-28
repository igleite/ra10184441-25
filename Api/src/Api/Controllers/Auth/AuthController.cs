using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Identity.Application.Commands.MagicLink;
using Identity.Application.Commands.Session;
using Identity.Application.Commands.VerificationToken;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Services.Auth;
using Tenant.Application.Commands.User;
using Tenant.Application.Interfaces;
using TenantUserDto = Tenant.Application.DTOs.UserDto;
using BuildingBlocks.Application.Enums;

namespace Api.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMediatorHandler _mediator;
    private readonly IAuthService _authService;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(
        IUserRepository userRepository,
        IMediatorHandler mediator,
        IAuthService authService,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _mediator = mediator;
        _authService = authService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<Result> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(request.Name.Trim(), request.Email.Trim(), RoleEnum.Onboarding);
        var user = await _mediator.SendCommand<CreateUserCommand, TenantUserDto>(command, cancellationToken);

        // envia o link para login
        await _mediator.SendCommand(new SendMagicLinkCommand(user.Email), cancellationToken);

        return Result.Factory.Success(StatusCodes.Status201Created);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<Result> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email.Trim());
        if (user is null)
            throw AppException.NotFound("Usuário não encontrado.");

        // envia o link para login
        await _mediator.SendCommand(new SendMagicLinkCommand(user.Email, request.AppOrigin), cancellationToken);

        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }

    [HttpGet("verify")]
    [AllowAnonymous]
    public async Task<Result> Verify([FromQuery] string token, CancellationToken cancellationToken)
    {
        var email = await _mediator.SendCommand<ConsumeVerificationTokenCommand, string>(new ConsumeVerificationTokenCommand(token), cancellationToken);

        var user = await _userRepository.GetByEmailAsync(email);
        if (user is null)
            throw AppException.NotFound("Usuário não encontrado.");

        var roleNames = await _authService.ResolveRoleNamesAsync(user, cancellationToken);
        //var extraClaims = await _authService.BuildExtraClaimsAsync(user, cancellationToken);

        var (jwtToken, expiresAt) = _jwtTokenService.GenerateToken(
            user.Id.ToString(),
            user.Name,
            user.Email,
            roleNames,
            null);

        await _mediator.SendCommand(new CreateSessionCommand(user.Id, jwtToken, expiresAt), cancellationToken);

        var login = new LoginDto
        {
            Token = jwtToken,
            ExpiresAt = expiresAt,
            User = _authService.ToUserDto(user, roleNames)
        };

        return Result.Factory<LoginDto>.Success(login);
    }

    [HttpGet("validate-session")]
    [AllowAnonymous]
    public async Task<Result> ValidateSession(CancellationToken cancellationToken)
    {
        var bearerToken = _authService.GetBearerToken();
        if (string.IsNullOrWhiteSpace(bearerToken))
            throw AppException.Unauthorized();

        var userId = await _mediator.SendCommand<ValidateSessionCommand, Guid>(new ValidateSessionCommand(bearerToken), cancellationToken);
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            throw AppException.Forbidden("Usuário não encontrado.");

        var roleNames = await _authService.ResolveRoleNamesAsync(user, cancellationToken);
        return Result.Factory<UserDto>.Success(_authService.ToUserDto(user, roleNames));
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<Result> Logout(CancellationToken cancellationToken)
    {
        var bearerToken = _authService.GetBearerToken();
        if (!string.IsNullOrWhiteSpace(bearerToken))
            await _mediator.SendCommand(new LogoutSessionCommand(bearerToken), cancellationToken);

        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}
