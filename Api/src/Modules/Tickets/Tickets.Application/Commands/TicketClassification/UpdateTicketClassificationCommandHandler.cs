using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;

namespace Tickets.Application.Commands.TicketClassification;

public class UpdateTicketClassificationCommandHandler : ICommandHandler<UpdateTicketClassificationCommand, TicketClassificationDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITicketClassificationRepository _repository;

    public UpdateTicketClassificationCommandHandler(IDateTimeProvider dateTimeProvider, ITicketClassificationRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<TicketClassificationDto> Handle(UpdateTicketClassificationCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;
        var code = request.Code.Trim().ToUpperInvariant();

        var codeExists = await _repository.GetByCodeAsync(request.OrganizationId, code);
        if (codeExists != null && codeExists.Id != request.Id)
            throw AppException.Conflict($"A classificação com código {code} já existe!");

        var nameExists = await _repository.GetByNameAsync(request.OrganizationId, request.Name);
        if (nameExists != null && nameExists.Id != request.Id)
            throw AppException.Conflict($"A classificação {request.Name} já existe!");

        var classification = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (classification is null)
            throw AppException.NotFound($"A classificação não existe!");

        if (!classification.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A classificação foi modificada por outro usuário. Recarregue a página e tente novamente.");

        classification.SetName(request.Name, dateNow);
        classification.SetCode(code, dateNow);

        var success = await _repository.UpdateAsync(classification);
        if (!success)
            throw AppException.BadRequest($"A classificação {request.Name} não foi atualizada!");

        return classification.ToDto();
    }
}
