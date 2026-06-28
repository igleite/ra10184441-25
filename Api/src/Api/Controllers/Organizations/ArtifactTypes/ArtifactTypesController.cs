using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.ArtifactType;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Queries.ArtifactType;
using Tenant.Application.Requests.ArtifactTypes;

namespace Api.Controllers.Organizations.ArtifactTypes;

[ApiController]
[Route("api/organizations/{organizationId}/artifact-types")]
public class ArtifactTypesController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IOrganizationRepository _organizationRepository;

    public ArtifactTypesController(IMediatorHandler mediator, IOrganizationRepository organizationRepository)
    {
        _mediator = mediator;
        _organizationRepository = organizationRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetArtifactTypeByPageQuery(organizationId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<ArtifactTypeDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetArtifactTypeByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<ArtifactTypeDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromBody] CreateArtifactTypeRequest request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(organizationId);
        if (organization is null)
            return Result.Factory<ArtifactTypeDto>.Error("A organização não existe!", StatusCodes.Status404NotFound);

        var command = new CreateArtifactTypeCommand(organizationId, request.Name);
        var result = await _mediator.SendCommand<CreateArtifactTypeCommand, ArtifactTypeDto>(command, cancellationToken);
        return Result.Factory<ArtifactTypeDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, Guid id, [FromBody] UpdateArtifactTypeRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateArtifactTypeCommand(organizationId, id, request.Name, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateArtifactTypeCommand, ArtifactTypeDto>(command, cancellationToken);
        return Result.Factory<ArtifactTypeDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteArtifactTypeCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteArtifactTypeCommand, ArtifactTypeDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}
