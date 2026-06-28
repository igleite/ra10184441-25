using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.OrganizationPlan;

public class UpdateOrganizationPlanCommandHandler : ICommandHandler<UpdateOrganizationPlanCommand, OrganizationPlanDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrganizationPlanRepository _repository;

    public UpdateOrganizationPlanCommandHandler(IDateTimeProvider dateTimeProvider, IOrganizationPlanRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<OrganizationPlanDto> Handle(UpdateOrganizationPlanCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var organizationPlan = await _repository.GetByIdAsync(request.Id);
        if (organizationPlan is null)
            throw AppException.NotFound($"O plano da organização não existe!");

        if (!organizationPlan.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O plano da organização foi modificado por outro usuário. Recarregue a página e tente novamente.");

        organizationPlan.SetPlanId(request.PlanId, dateNow);
        organizationPlan.SetDescription(request.Description, dateNow);
        organizationPlan.SetMaxUsers(request.MaxUsers, dateNow);
        organizationPlan.SetMaxClients(request.MaxClients, dateNow);
        organizationPlan.SetMaxTickets(request.MaxTickets, dateNow);

        var success = await _repository.UpdateAsync(organizationPlan);
        if (!success)
            throw AppException.BadRequest($"O plano da organização não foi atualizado!");

        return OrganizationPlanMappings.ToDto(organizationPlan);
    }
}

