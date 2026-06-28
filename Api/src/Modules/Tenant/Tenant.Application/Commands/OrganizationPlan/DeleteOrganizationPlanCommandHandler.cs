using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.OrganizationPlan;

public class DeleteOrganizationPlanCommandHandler : ICommandHandler<DeleteOrganizationPlanCommand, OrganizationPlanDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrganizationPlanRepository _repository;

    public DeleteOrganizationPlanCommandHandler(IDateTimeProvider dateTimeProvider, IOrganizationPlanRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<OrganizationPlanDto> Handle(DeleteOrganizationPlanCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var organizationPlan = await _repository.GetByIdAsync(request.Id);
        if (organizationPlan is null || organizationPlan.OrganizationId != request.OrganizationId)
            throw AppException.NotFound($"O plano da organização não existe!");

        if (!organizationPlan.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O plano da organização foi modificado por outro usuário. Recarregue a página e tente novamente.");

        organizationPlan.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(organizationPlan);
        if (!success)
            throw AppException.BadRequest($"O plano da organização não foi deletado!");

        return OrganizationPlanMappings.ToDto(organizationPlan);
    }
}

