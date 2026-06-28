using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Interfaces;
using Tickets.Application.Commands.TicketClassification;
using Tickets.Application.DTOs;
using Tickets.Application.Queries.TicketClassification;
using Tickets.Application.Requests.TicketClassifications;

namespace Api.Controllers.Organizations.Tickets;

[ApiController]
[Route("api/organizations/{organizationId}/ticket-classifications")]
public class TicketClassificationsController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IOrganizationRepository _organizationRepository;

    public TicketClassificationsController(IMediatorHandler mediator, IOrganizationRepository organizationRepository)
    {
        _mediator = mediator;
        _organizationRepository = organizationRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetTicketClassificationByPageQuery(organizationId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<TicketClassificationDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTicketClassificationByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<TicketClassificationDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromBody] CreateTicketClassificationRequest request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(organizationId);
        if (organization is null)
            return Result.Factory<TicketClassificationDto>.Error("A organização não existe!", StatusCodes.Status404NotFound);

        var command = new CreateTicketClassificationCommand(organizationId, request.Name, request.Code);
        var result = await _mediator.SendCommand<CreateTicketClassificationCommand, TicketClassificationDto>(command, cancellationToken);
        return Result.Factory<TicketClassificationDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, Guid id, [FromBody] UpdateTicketClassificationRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTicketClassificationCommand(organizationId, id, request.Name, request.Code, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateTicketClassificationCommand, TicketClassificationDto>(command, cancellationToken);
        return Result.Factory<TicketClassificationDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteTicketClassificationCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteTicketClassificationCommand, TicketClassificationDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}
