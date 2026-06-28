using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using BuildingBlocks.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.OrganizationUser;
using Tenant.Application.DTOs;
using Tenant.Application.Queries.OrganizationUser;
using Tenant.Application.Requests.OrganizationUsers;
using Tenant.Application.Interfaces;

namespace Api.Controllers.Organizations.OrganizationsUsers;

[ApiController]
[Route("api/organizations/{organizationId}/users")]
public class OrganizationsUsersController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IUserRepository _userRepository;

    public OrganizationsUsersController(IMediatorHandler mediator, IUserRepository userRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetOrganizationUserByPageQuery(organizationId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<OrganizationUserDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrganizationUserByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<OrganizationUserDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromBody] CreateOrganizationUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Factory<OrganizationUserDto>.Error("O usuário não existe!", StatusCodes.Status404NotFound);

        var command = new CreateOrganizationUserCommand(organizationId, request.UserId, request.TeamId);
        var result = await _mediator.SendCommand<CreateOrganizationUserCommand, OrganizationUserDto>(command, cancellationToken);
        return Result.Factory<OrganizationUserDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, Guid id, [FromBody] UpdateOrganizationUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Factory<OrganizationUserDto>.Error("O usuário não existe!", StatusCodes.Status404NotFound);

        var command = new UpdateOrganizationUserCommand(organizationId, id, request.UserId, request.TeamId, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateOrganizationUserCommand, OrganizationUserDto>(command, cancellationToken);
        return Result.Factory<OrganizationUserDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteOrganizationUserCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteOrganizationUserCommand, OrganizationUserDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}

