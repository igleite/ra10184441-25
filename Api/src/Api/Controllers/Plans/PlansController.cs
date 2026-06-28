using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.Plan;
using Tenant.Application.DTOs;
using Tenant.Application.Queries.Plan;
using Tenant.Application.Requests.Plans;

namespace Api.Controllers.Plans;

[ApiController]
[Route("api/plans")]
public class PlansController : ControllerBase
{
    private readonly IMediatorHandler _mediator;

    public PlansController(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<Result> Get([FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetPlanByPageQuery(pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<PlanDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<Result> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPlanByIdQuery(id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PlanDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromBody] CreatePlanRequest request, CancellationToken cancellationToken)
    {
        var command = new CreatePlanCommand(
            request.Name,
            request.Description,
            request.MaxUsers,
            request.MaxClients,
            request.MaxTickets);
        var result = await _mediator.SendCommand<CreatePlanCommand, PlanDto>(command, cancellationToken);
        return Result.Factory<PlanDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put(Guid id, [FromBody] UpdatePlanRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdatePlanCommand(
            id,
            request.Name,
            request.Description,
            request.MaxUsers,
            request.MaxClients,
            request.MaxTickets,
            request.RowVersion);
        var result = await _mediator.SendCommand<UpdatePlanCommand, PlanDto>(command, cancellationToken);
        return Result.Factory<PlanDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete(Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeletePlanCommand(id, rowVersion);
        await _mediator.SendCommand<DeletePlanCommand, PlanDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}

