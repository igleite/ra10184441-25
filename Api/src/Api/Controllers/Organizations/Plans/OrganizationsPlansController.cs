using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant.Application.Commands.OrganizationPlan;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Queries.OrganizationPlan;
using Tenant.Application.Requests.OrganizationPlans;

namespace Api.Controllers.Organizations.Plans;

[ApiController]
[Route("api/organizations/{organizationId}/plans")]
public class OrganizationsPlansController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IPlanRepository _planRepository;

    public OrganizationsPlansController(IMediatorHandler mediator, IOrganizationRepository organizationRepository, IPlanRepository planRepository)
    {
        _mediator = mediator;
        _organizationRepository = organizationRepository;
        _planRepository = planRepository;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Get([FromRoute] Guid organizationId, [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetOrganizationPlanByPageQuery(organizationId, pageIndex, pageSize);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<PageDto<OrganizationPlanDto>>.Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> GetById([FromRoute] Guid organizationId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrganizationPlanByIdQuery(organizationId, id);
        var result = await _mediator.Query(query, cancellationToken);
        return Result.Factory<OrganizationPlanDto>.Success(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Post([FromRoute] Guid organizationId, [FromBody] CreateOrganizationPlanRequest request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(organizationId);
        if (organization is null)
            return Result.Factory<OrganizationPlanDto>.Error("A organização não existe!", StatusCodes.Status404NotFound);

        var plan = await _planRepository.GetByIdAsync(request.PlanId);
        if (plan is null)
            return Result.Factory<OrganizationPlanDto>.Error("O plano não existe!", StatusCodes.Status404NotFound);

        var command = new CreateOrganizationPlanCommand(
            organizationId,
            request.PlanId,
            request.Description,
            request.MaxUsers,
            request.MaxClients,
            request.MaxTickets);

        var result = await _mediator.SendCommand<CreateOrganizationPlanCommand, OrganizationPlanDto>(command, cancellationToken);
        return Result.Factory<OrganizationPlanDto>.Success(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Put([FromRoute] Guid organizationId, Guid id, [FromBody] UpdateOrganizationPlanRequest request, CancellationToken cancellationToken)
    {
        var plan = await _planRepository.GetByIdAsync(request.PlanId);
        if (plan is null)
            return Result.Factory<OrganizationPlanDto>.Error("O plano não existe!", StatusCodes.Status404NotFound);

        var command = new UpdateOrganizationPlanCommand(
            organizationId,
            id,
            request.PlanId,
            request.Description,
            request.MaxUsers,
            request.MaxClients,
            request.MaxTickets,
            request.RowVersion);

        var result = await _mediator.SendCommand<UpdateOrganizationPlanCommand, OrganizationPlanDto>(command, cancellationToken);
        return Result.Factory<OrganizationPlanDto>.Success(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.Default)]
    public async Task<Result> Delete([FromRoute] Guid organizationId, Guid id, [FromQuery] byte[] rowVersion, CancellationToken cancellationToken)
    {
        var command = new DeleteOrganizationPlanCommand(organizationId, id, rowVersion);
        await _mediator.SendCommand<DeleteOrganizationPlanCommand, OrganizationPlanDto>(command, cancellationToken);
        return Result.Factory.Success(StatusCodes.Status204NoContent);
    }
}

