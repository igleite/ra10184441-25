using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tenant.Application.Commands.Organization;
using Tenant.Application.Commands.OrganizationUser;
using Tenant.Application.Commands.Team;
using Tenant.Application.Commands.User;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Queries.Organization;
using Tenant.Application.Requests.Organizations;
using Tenant.Domain.Entities;
using Tickets.Application.Commands.StatusReason;
using Tickets.Application.DTOs;
using Tickets.Domain.ValueObjects;

namespace Api.Controllers.Organizations;

[ApiController]
[Route("api/organizations")]
public class OrganizationsController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IUserRepository _userRepository;

    public OrganizationsController(IMediatorHandler mediator, IUserRepository userRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<Result> Get([FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetOrganizationByPageQuery(pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<OrganizationDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<Result> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrganizationByIdQuery(id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<OrganizationDto>.Success(result);
    }

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<Result> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var query = new GetOrganizationBySlugQuery(slug);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<OrganizationDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromBody] CreateOrganizationRequest request, CancellationToken cancellationToken)
    {
        var userIdRaw = User.FindFirstValue(ClaimTypeEnum.UserId.Type) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdRaw, out var userId))
            throw AppException.Unauthorized();

        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            throw AppException.Forbidden("Usuário não encontrado.");

        var isOnboarding = user.RoleId == RoleEnum.Onboarding.Id;
        if (isOnboarding)
        {
            var commandUser = new UpdateUserCommand(user.Id, user.Name, user.Email, RoleEnum.Nullable, user.RowVersion);
            await _mediator.SendCommand<UpdateUserCommand, UserDto>(commandUser, cancellationToken);
        }

        var organizationDto = await _mediator.SendCommand<CreateOrganizationCommand, OrganizationDto>(new CreateOrganizationCommand(request.Name, request.Document, request.Slug), cancellationToken);
        var teamDto = await _mediator.SendCommand<CreateTeamCommand, TeamDto>(new CreateTeamCommand(organizationDto.Id, TeamEnum.OrganizationAdmin.Name, TeamEnum.OrganizationAdmin.Code, RoleEnum.OrganizationOwner.Id), cancellationToken);
        await _mediator.SendCommand<CreateOrganizationUserCommand, OrganizationUserDto>(new CreateOrganizationUserCommand(organizationDto.Id, user.Id, teamDto.Id), cancellationToken);
        await _mediator.SendCommand<CreateStatusReasonCommand, StatusReasonDto>(new CreateStatusReasonCommand(organizationDto.Id, StatusType.Open, "Aberto", true), cancellationToken);
        await _mediator.SendCommand<CreateStatusReasonCommand, StatusReasonDto>(new CreateStatusReasonCommand(organizationDto.Id, StatusType.Open, "Fechado"), cancellationToken);

        return Result.Factory<OrganizationDto>.Success(organizationDto);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put(Guid id, [FromBody] UpdateOrganizationRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateOrganizationCommand(id, request.Name, request.Document, request.Slug, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateOrganizationCommand, OrganizationDto>(command, cancellationToken);
        return Result.Factory<OrganizationDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete(Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteOrganizationCommand(id, rowVersion);
        await _mediator.SendCommand<DeleteOrganizationCommand, OrganizationDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}

