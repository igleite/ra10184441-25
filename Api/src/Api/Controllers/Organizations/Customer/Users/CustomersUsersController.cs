using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using BuildingBlocks.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.CustomerUser;
using Tenant.Application.DTOs;
using Tenant.Application.Queries.CustomerUser;
using Tenant.Application.Requests.CustomerUsers;
using Tenant.Application.Interfaces;

namespace Api.Controllers.Organizations.Customer.Users;

[ApiController]
[Route("api/organizations/{organizationId}/customers/{customerId}/users")]
public class CustomersUsersController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;

    public CustomersUsersController(IMediatorHandler mediator, IUserRepository userRepository, ICustomerRepository customerRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
        _customerRepository = customerRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromRoute] Guid customerId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetCustomerUserByPageQuery(organizationId, customerId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<CustomerUserDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCustomerUserByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<CustomerUserDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromRoute] Guid customerId, [FromBody] CreateCustomerUserRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(organizationId, customerId);
        if (customer is null)
            return Result.Factory<CustomerUserDto>.Error("O cliente não existe!", StatusCodes.Status404NotFound);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Factory<CustomerUserDto>.Error("O usuário não existe!", StatusCodes.Status404NotFound);

        var command = new CreateCustomerUserCommand(organizationId, customerId, request.UserId);
        var result = await _mediator.SendCommand<CreateCustomerUserCommand, CustomerUserDto>(command, cancellationToken);
        return Result.Factory<CustomerUserDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, [FromRoute] Guid customerId, Guid id, [FromBody] UpdateCustomerUserRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(organizationId, customerId);
        if (customer is null)
            return Result.Factory<CustomerUserDto>.Error("O cliente não existe!", StatusCodes.Status404NotFound);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Factory<CustomerUserDto>.Error("O usuário não existe!", StatusCodes.Status404NotFound);

        var command = new UpdateCustomerUserCommand(organizationId, id, customerId, request.UserId, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateCustomerUserCommand, CustomerUserDto>(command, cancellationToken);
        return Result.Factory<CustomerUserDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteCustomerUserCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteCustomerUserCommand, CustomerUserDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}

