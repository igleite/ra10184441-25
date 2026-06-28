using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using FeatureFlags.Application.Commands;
using FeatureFlags.Application.DTOs;
using FeatureFlags.Application.Queries;
using FeatureFlags.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.FeatureFlags;

[ApiController]
[Route("api/feature-flags")]
public class FeatureFlagsController : ControllerBase
{
    private readonly IMediatorHandler _mediator;

    public FeatureFlagsController(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetFeatureFlagByPageQuery(pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<FeatureFlagDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetFeatureFlagByIdQuery(id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<FeatureFlagDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromBody] CreateFeatureFlagRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateFeatureFlagCommand(request.Name, request.Description);
        var result = await _mediator.SendCommand<CreateFeatureFlagCommand, FeatureFlagDto>(command, cancellationToken);
        return Result.Factory<FeatureFlagDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put(Guid id, [FromBody] UpdateFeatureFlagRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateFeatureFlagCommand(id, request.Name, request.Description, request.Value, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateFeatureFlagCommand, FeatureFlagDto>(command, cancellationToken);
        return Result.Factory<FeatureFlagDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete(Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteFeatureFlagCommand(id, rowVersion);
        await _mediator.SendCommand<DeleteFeatureFlagCommand, FeatureFlagDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}

