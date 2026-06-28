using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.Plan;

public class UpdatePlanCommandHandler : ICommandHandler<UpdatePlanCommand, PlanDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPlanRepository _repository;

    public UpdatePlanCommandHandler(IDateTimeProvider dateTimeProvider, IPlanRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<PlanDto> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var plan = await _repository.GetByIdAsync(request.Id);
        if (plan is null)
            throw AppException.NotFound($"O plano não existe!");

        if (!plan.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O plano foi modificado por outro usuário. Recarregue a página e tente novamente.");

        var nameExists = await _repository.GetByNameAsync(request.Name);
        if (nameExists != null && nameExists.Id != request.Id)
            throw AppException.Conflict($"O plano {request.Name} já existe!");

        plan.SetName(request.Name, dateNow);
        plan.SetDescription(request.Description, dateNow);
        plan.SetMaxUsers(request.MaxUsers, dateNow);
        plan.SetMaxClients(request.MaxClients, dateNow);
        plan.SetMaxTickets(request.MaxTickets, dateNow);

        var success = await _repository.UpdateAsync(plan);
        if (!success)
            throw AppException.BadRequest($"O plano não foi atualizado!");

        return PlanMappings.ToDto(plan);
    }
}

