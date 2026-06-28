using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.OrganizationPlan;

public class CreateOrganizationPlanCommandHandler : ICommandHandler<CreateOrganizationPlanCommand, OrganizationPlanDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrganizationPlanRepository _repository;
    private readonly IPlanRepository _planRepository;

    public CreateOrganizationPlanCommandHandler(IDateTimeProvider dateTimeProvider, IOrganizationPlanRepository repository, IPlanRepository planRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
        _planRepository = planRepository;
    }

    public async Task<OrganizationPlanDto> Handle(CreateOrganizationPlanCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var plan = await _planRepository.GetByIdAsync(request.PlanId);
        if (plan is null)
            throw AppException.NotFound($"O plano não existe!");

        var organizationPlan = new Domain.Entities.OrganizationPlan(Guid.NewGuid(), dateNow, request.OrganizationId);
        organizationPlan.SetPlanId(request.PlanId, dateNow);
        organizationPlan.SetDescription(request.Description, dateNow);
        organizationPlan.SetMaxUsers(request.MaxUsers, dateNow);
        organizationPlan.SetMaxClients(request.MaxClients, dateNow);
        organizationPlan.SetMaxTickets(request.MaxTickets, dateNow);

        var success = await _repository.CreateAsync(organizationPlan);
        if (!success)
            throw AppException.BadRequest($"O plano da organização não foi inserido!");

        return OrganizationPlanMappings.ToDto(organizationPlan);
    }
}

