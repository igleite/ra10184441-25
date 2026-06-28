using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.Plan;

public class CreatePlanCommandHandler : ICommandHandler<CreatePlanCommand, PlanDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPlanRepository _repository;

    public CreatePlanCommandHandler(IDateTimeProvider dateTimeProvider, IPlanRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<PlanDto> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var nameExists = await _repository.GetByNameAsync(request.Name);
        if (nameExists != null)
            throw AppException.Conflict($"O plano {request.Name} já existe!");

        var plan = new Domain.Entities.Plan(Guid.NewGuid(), dateNow);
        plan.SetName(request.Name, dateNow);
        plan.SetDescription(request.Description, dateNow);
        plan.SetMaxUsers(request.MaxUsers, dateNow);
        plan.SetMaxClients(request.MaxClients, dateNow);
        plan.SetMaxTickets(request.MaxTickets, dateNow);

        var success = await _repository.CreateAsync(plan);
        if (!success)
            throw AppException.BadRequest($"O plano {request.Name} não foi inserido!");

        return PlanMappings.ToDto(plan);
    }
}

