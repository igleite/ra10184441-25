using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using BuildingBlocks.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.CustomerArtifact;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Queries.CustomerArtifact;
using Tenant.Application.Requests.CustomerArtifacts;

namespace Api.Controllers.Organizations.Customer.Artifacts;

[ApiController]
[Route("api/organizations/{organizationId}/customers/{customerId}/artifacts")]
public class CustomerArtifactsController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ICustomerRepository _customerRepository;
    private readonly IArtifactRepository _artifactRepository;

    public CustomerArtifactsController(
        IMediatorHandler mediator,
        ICustomerRepository customerRepository,
        IArtifactRepository artifactRepository)
    {
        _mediator = mediator;
        _customerRepository = customerRepository;
        _artifactRepository = artifactRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromRoute] Guid customerId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetCustomerArtifactByPageQuery(organizationId, customerId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<CustomerArtifactDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCustomerArtifactByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<CustomerArtifactDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromRoute] Guid customerId, [FromBody] CreateCustomerArtifactRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(organizationId, customerId);
        if (customer is null || customer.OrganizationId != organizationId)
            return Result.Factory<CustomerArtifactDto>.Error("O cliente não existe!", StatusCodes.Status404NotFound);

        var artifact = await _artifactRepository.GetByIdAsync(organizationId, request.ArtifactId);
        if (artifact is null)
            return Result.Factory<CustomerArtifactDto>.Error("O artefato não existe!", StatusCodes.Status404NotFound);

        var command = new CreateCustomerArtifactCommand(organizationId, customerId, request.ArtifactId);
        var result = await _mediator.SendCommand<CreateCustomerArtifactCommand, CustomerArtifactDto>(command, cancellationToken);
        return Result.Factory<CustomerArtifactDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, [FromRoute] Guid customerId, Guid id, [FromBody] UpdateCustomerArtifactRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(organizationId, customerId);
        if (customer is null || customer.OrganizationId != organizationId)
            return Result.Factory<CustomerArtifactDto>.Error("O cliente não existe!", StatusCodes.Status404NotFound);

        var artifact = await _artifactRepository.GetByIdAsync(organizationId, request.ArtifactId);
        if (artifact is null)
            return Result.Factory<CustomerArtifactDto>.Error("O artefato não existe!", StatusCodes.Status404NotFound);

        var command = new UpdateCustomerArtifactCommand(organizationId, id, customerId, request.ArtifactId, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateCustomerArtifactCommand, CustomerArtifactDto>(command, cancellationToken);
        return Result.Factory<CustomerArtifactDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteCustomerArtifactCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteCustomerArtifactCommand, CustomerArtifactDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}
