using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;
using System.Linq;

namespace Tickets.Application.Commands.StatusReason;

public class DeleteStatusReasonCommandHandler : ICommandHandler<DeleteStatusReasonCommand, StatusReasonDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IStatusReasonRepository _repository;

    public DeleteStatusReasonCommandHandler(IDateTimeProvider dateTimeProvider, IStatusReasonRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<StatusReasonDto> Handle(DeleteStatusReasonCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var statusReason = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (statusReason is null)
            throw AppException.NotFound($"O motivo de status não existe!");

        if (!statusReason.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O motivo de status foi modificado por outro usuário. Recarregue a página e tente novamente.");

        statusReason.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(statusReason);
        if (!success)
            throw AppException.BadRequest($"O motivo de status não foi deletado!");

        return StatusReasonMappings.ToDto(statusReason);
    }
}

