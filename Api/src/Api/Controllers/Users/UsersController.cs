using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.User;
using Tenant.Application.DTOs;
using Tenant.Application.Queries.User;
using Tenant.Application.Requests.Users;

namespace Api.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediatorHandler _mediator;

    public UsersController(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetUserByPageQuery(pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<UserDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<UserDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        // TODO - REVISAR
        var command = new CreateUserCommand(request.Name, request.Email, RoleEnum.OrganizationMember);
        var result = await _mediator.SendCommand<CreateUserCommand, UserDto>(command, cancellationToken);
        return Result.Factory<UserDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var role = request.Role ?? RoleEnum.Unchanged;
        var command = new UpdateUserCommand(id, request.Name, request.Email, role, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateUserCommand, UserDto>(command, cancellationToken);
        return Result.Factory<UserDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete(Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id, rowVersion);
        await _mediator.SendCommand<DeleteUserCommand, UserDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}

