using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;

namespace Tickets.Application.Commands.StatusReason;

public class CreateStatusReasonCommandHandler : ICommandHandler<CreateStatusReasonCommand, StatusReasonDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IStatusReasonRepository _repository;

    public CreateStatusReasonCommandHandler(IDateTimeProvider dateTimeProvider, IStatusReasonRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<StatusReasonDto> Handle(CreateStatusReasonCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var statusReasonExiste = await _repository.GetByNameAsync(request.OrganizationId, request.Name);
        if (statusReasonExiste != null)
            throw AppException.Conflict($"O motivo de status {request.Name} já existe!");

        var statusReason = new Domain.Entities.StatusReason(Guid.NewGuid(), dateNow, request.OrganizationId);
        statusReason.SetType(request.Type, dateNow);
        statusReason.SetName(request.Name, dateNow);
        statusReason.SetIsOpeningDefault(request.IsOpeningDefault, dateNow);

        var success = await _repository.CreateAsync(statusReason);
        if (!success)
            throw AppException.BadRequest($"O motivo de status {request.Name} não foi inserido!");

        return StatusReasonMappings.ToDto(statusReason);
    }
}

