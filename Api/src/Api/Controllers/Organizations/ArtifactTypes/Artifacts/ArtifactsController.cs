using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.Artifact;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Queries.Artifact;
using Tenant.Application.Requests.Artifacts;

namespace Api.Controllers.Organizations.ArtifactTypes.Artifacts;

[ApiController]
[Route("api/organizations/{organizationId}/artifact-types/{artifactTypeId}/artifacts")]
public class ArtifactsController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IArtifactTypeRepository _artifactTypeRepository;

    public ArtifactsController(
        IMediatorHandler mediator,
        IOrganizationRepository organizationRepository,
        IArtifactTypeRepository artifactTypeRepository)
    {
        _mediator = mediator;
        _organizationRepository = organizationRepository;
        _artifactTypeRepository = artifactTypeRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromRoute] Guid artifactTypeId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetArtifactByPageQuery(organizationId, artifactTypeId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<ArtifactDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetArtifactByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<ArtifactDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromRoute] Guid artifactTypeId, [FromBody] CreateArtifactRequest request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(organizationId);
        if (organization is null)
            return Result.Factory<ArtifactDto>.Error("A organização não existe!", StatusCodes.Status404NotFound);

        var artifactType = await _artifactTypeRepository.GetByIdAsync(organizationId, artifactTypeId);
        if (artifactType is null)
            return Result.Factory<ArtifactDto>.Error("O tipo de artefato não existe!", StatusCodes.Status404NotFound);

        var command = new CreateArtifactCommand(organizationId, artifactTypeId, request.Name, request.Code);
        var result = await _mediator.SendCommand<CreateArtifactCommand, ArtifactDto>(command, cancellationToken);
        return Result.Factory<ArtifactDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, Guid id, [FromBody] UpdateArtifactRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateArtifactCommand(organizationId, id, request.ArtifactTypeId, request.Name, request.Code, request.RowVersion);
        var result = await _mediator.SendCommand<UpdateArtifactCommand, ArtifactDto>(command, cancellationToken);
        return Result.Factory<ArtifactDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteArtifactCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteArtifactCommand, ArtifactDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}
