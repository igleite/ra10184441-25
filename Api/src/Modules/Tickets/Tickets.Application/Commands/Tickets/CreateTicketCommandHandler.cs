using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.Commands.Tickets;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;
using Tickets.Domain.Entities;
using Tickets.Domain.ValueObjects;

namespace Tickets.Application.Commands;

public class CreateTicketCommandHandler : ICommandHandler<CreateTicketCommand, TicketDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITicketRepository _repository;
    private readonly IStatusReasonRepository _statusReasonRepository;
    private readonly ITicketClassificationRepository _ticketClassificationRepository;

    public CreateTicketCommandHandler(
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

    public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var ticket = new Ticket(Guid.NewGuid(), dateNow, request.OrganizationId);

        ticket.SetCustomerId(request.CustomerId, dateNow);
        ticket.SetArtifactId(request.ArtifactId, dateNow);

        var status = await _statusReasonRepository.GetByOpeningDefaultAsync(request.OrganizationId);
        if (status is null)
            throw AppException.NotFound($"Não existe um status padrão de abertura configurado para esta organização!");

        var classification = await _ticketClassificationRepository.GetByIdAsync(request.OrganizationId, request.ClassificationId);
        if (classification is null)
            throw AppException.NotFound($"A classificação do ticket não existe!");

        ticket.SetStatusId(status.Id, dateNow);
        ticket.SetClassificationId(classification.Id, dateNow);
        ticket.SetAllocationCenter(AllocationCenter.Customer, dateNow);
        ticket.SetCreatedByUserId(request.CreatedByUserId, dateNow);
        ticket.SetDescription(request.Description, dateNow);

        var success = await _repository.CreateAsync(ticket);
        if (!success)
            throw AppException.BadRequest($"O ticket não foi inserido!");

        return TicketMappings.ToDto(ticket);
    }
}
