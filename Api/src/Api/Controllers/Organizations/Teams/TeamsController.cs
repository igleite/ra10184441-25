using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.Team;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Queries.Team;
using Tenant.Application.Requests.Teams;

namespace Api.Controllers.Organizations.Teams;

[ApiController]
[Route("api/organizations/{organizationId}/teams")]
public class TeamsController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IOrganizationRepository _organizationRepository;

    public TeamsController(IMediatorHandler mediator, IOrganizationRepository organizationRepository)
    {
        _mediator = mediator;
        _organizationRepository = organizationRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetTeamByPageQuery(organizationId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<TeamDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTeamByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<TeamDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromBody] CreateTeamRequest request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(organizationId);
        if (organization is null)
            return Result.Factory<TeamDto>.Error("A organização não existe!", StatusCodes.Status404NotFound);

        var command = new CreateTeamCommand(organizationId, request.Name, request.Code, request.RoleId);
        var result = await _mediator.SendCommand<CreateTeamCommand, TeamDto>(command, cancellationToken);
        return Result.Factory<TeamDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, Guid id, [FromBody] UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTeamCommand(organizationId, id, request.Name, request.Code, request.RoleId, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateTeamCommand, TeamDto>(command, cancellationToken);
        return Result.Factory<TeamDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteTeamCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteTeamCommand, TeamDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}
