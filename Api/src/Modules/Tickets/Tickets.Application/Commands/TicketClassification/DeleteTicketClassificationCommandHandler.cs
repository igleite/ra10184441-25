using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;

namespace Tickets.Application.Commands.TicketClassification;

public class DeleteTicketClassificationCommandHandler : ICommandHandler<DeleteTicketClassificationCommand, TicketClassificationDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITicketClassificationRepository _repository;

    public DeleteTicketClassificationCommandHandler(IDateTimeProvider dateTimeProvider, ITicketClassificationRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<TicketClassificationDto> Handle(DeleteTicketClassificationCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var classification = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (classification is null)
            throw AppException.NotFound($"A classificação não existe!");

        if (!classification.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A classificação foi modificada por outro usuário. Recarregue a página e tente novamente.");

        classification.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(classification);
        if (!success)
            throw AppException.BadRequest($"A classificação não foi deletada!");

        return classification.ToDto();
    }
}
