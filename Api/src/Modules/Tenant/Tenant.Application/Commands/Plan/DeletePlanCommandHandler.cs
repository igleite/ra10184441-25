using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.Plan;

public class DeletePlanCommandHandler : ICommandHandler<DeletePlanCommand, PlanDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPlanRepository _repository;

    public DeletePlanCommandHandler(IDateTimeProvider dateTimeProvider, IPlanRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<PlanDto> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var plan = await _repository.GetByIdAsync(request.Id);
        if (plan is null)
            throw AppException.NotFound($"O plano não existe!");

        if (!plan.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O plano foi modificado por outro usuário. Recarregue a página e tente novamente.");

        plan.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(plan);
        if (!success)
            throw AppException.BadRequest($"O plano não foi deletado!");

        return PlanMappings.ToDto(plan);
    }
}

