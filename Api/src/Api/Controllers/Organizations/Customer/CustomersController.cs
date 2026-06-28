using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using BuildingBlocks.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.Customer;
using Tenant.Application.DTOs;
using Tenant.Application.Queries.Customer;
using Tenant.Application.Requests.Customers;
using Tenant.Application.Interfaces;

namespace Api.Controllers.Organizations.Customer;

[ApiController]
[Route("api/organizations/{organizationId}/customers")]
public class CustomersController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IOrganizationRepository _organizationRepository;

    public CustomersController(IMediatorHandler mediator, IOrganizationRepository organizationRepository)
    {
        _mediator = mediator;
        _organizationRepository = organizationRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetCustomerByPageQuery(organizationId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<CustomerDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCustomerByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<CustomerDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(organizationId);
        if (organization is null)
            return Result.Factory<CustomerDto>.Error("A organização não existe!", StatusCodes.Status404NotFound);

        var command = new CreateCustomerCommand(organizationId, request.Name, request.Document);
        var result = await _mediator.SendCommand<CreateCustomerCommand, CustomerDto>(command, cancellationToken);
        return Result.Factory<CustomerDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerCommand(organizationId, id, request.Name, request.Document, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateCustomerCommand, CustomerDto>(command, cancellationToken);
        return Result.Factory<CustomerDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteCustomerCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteCustomerCommand, CustomerDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}

