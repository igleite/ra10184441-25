using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Interfaces;
using Tickets.Application.Commands.StatusReason;
using Tickets.Application.DTOs;
using Tickets.Application.Queries.StatusReason;
using Tickets.Application.Requests.StatusReasons;
using Tickets.Domain.ValueObjects;

namespace Api.Controllers.Organizations.Statuses;

[ApiController]
[Route("api/organizations/{organizationId}/ticket-status-reasons")]
public class StatusReasonsController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IOrganizationRepository _organizationRepository;

    public StatusReasonsController(IMediatorHandler mediator, IOrganizationRepository organizationRepository)
    {
        _mediator = mediator;
        _organizationRepository = organizationRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetStatusReasonByPageQuery(organizationId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<StatusReasonDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetStatusReasonByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<StatusReasonDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromBody] CreateStatusReasonRequest request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(organizationId);
        if (organization is null)
            return Result.Factory<StatusReasonDto>.Error("A organização não existe!", StatusCodes.Status404NotFound);

        var command = new CreateStatusReasonCommand(organizationId, StatusType.From(request.Type), request.Name);

        var result = await _mediator.SendCommand<CreateStatusReasonCommand, StatusReasonDto>(command, cancellationToken);
        return Result.Factory<StatusReasonDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, Guid id, [FromBody] UpdateStatusReasonRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateStatusReasonCommand(organizationId, id, StatusType.From(request.Type), request.Name, request.RowVersion);

        var result = await _mediator.SendCommand<UpdateStatusReasonCommand, StatusReasonDto>(command, cancellationToken);
        return Result.Factory<StatusReasonDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteStatusReasonCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteStatusReasonCommand, StatusReasonDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}

