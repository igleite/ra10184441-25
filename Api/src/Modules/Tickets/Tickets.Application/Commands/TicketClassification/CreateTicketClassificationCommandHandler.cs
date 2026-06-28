using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;

namespace Tickets.Application.Commands.TicketClassification;

public class CreateTicketClassificationCommandHandler : ICommandHandler<CreateTicketClassificationCommand, TicketClassificationDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITicketClassificationRepository _repository;

    public CreateTicketClassificationCommandHandler(IDateTimeProvider dateTimeProvider, ITicketClassificationRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<TicketClassificationDto> Handle(CreateTicketClassificationCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;
        var code = request.Code.Trim().ToUpperInvariant();

        var codeExists = await _repository.GetByCodeAsync(request.OrganizationId, code);
        if (codeExists != null)
            throw AppException.Conflict($"A classificação com código {code} já existe!");

        var nameExists = await _repository.GetByNameAsync(request.OrganizationId, request.Name);
        if (nameExists != null)
            throw AppException.Conflict($"A classificação {request.Name} já existe!");

        var classification = new Domain.Entities.TicketClassification(Guid.NewGuid(), dateNow, request.OrganizationId);
        classification.SetName(request.Name, dateNow);
        classification.SetCode(code, dateNow);

        var success = await _repository.CreateAsync(classification);
        if (!success)
            throw AppException.BadRequest($"A classificação {request.Name} não foi inserida!");

        return classification.ToDto();
    }
}
