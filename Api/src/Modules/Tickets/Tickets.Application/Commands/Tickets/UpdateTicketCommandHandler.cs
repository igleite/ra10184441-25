using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.Commands.Tickets;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;
using Tickets.Domain.ValueObjects;

namespace Tickets.Application.Commands;

public class UpdateTicketCommandHandler : ICommandHandler<UpdateTicketCommand, TicketDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITicketRepository _repository;
    private readonly IStatusReasonRepository _statusReasonRepository;
    private readonly ITicketClassificationRepository _ticketClassificationRepository;

    public UpdateTicketCommandHandler(
        IDateTimeProvider dateTimeProvider,
        ITicketRepository repository,
        IStatusReasonRepository statusReasonRepository,
        ITicketClassificationRepository ticketClassificationRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
        _statusReasonRepository = statusReasonRepository;
        _ticketClassificationRepository = ticketClassificationRepository;
    }

    public async Task<TicketDto> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var ticket = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (ticket is null)
            throw AppException.NotFound($"O ticket {request.Id} não existe!");

        if (!ticket.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O ticket foi modificado por outro usuário. Recarregue a página e tente novamente.");

        var status = await _statusReasonRepository.GetByIdAsync(request.OrganizationId, request.StatusId);
        if (status is null)
            throw AppException.NotFound($"O status não existe!");

        var classification = await _ticketClassificationRepository.GetByIdAsync(request.OrganizationId, request.ClassificationId);
        if (classification is null)
            throw AppException.NotFound($"A classificação do ticket não existe!");

        ticket.SetStatusId(request.StatusId, dateNow);
        ticket.SetClassificationId(request.ClassificationId, dateNow);
        ticket.SetArtifactId(request.ArtifactId, dateNow);
        ticket.SetAllocationCenter(AllocationCenter.From(request.AllocationCenter), dateNow);
        ticket.SetDescription(request.Description, dateNow);
        ticket.SetResolution(request.Resolution, dateNow);

        var success = await _repository.UpdateAsync(ticket);
        if (!success)
            throw AppException.BadRequest($"O ticket {request.Id} não foi atualizado!");

        return TicketMappings.ToDto(ticket);
    }
}
