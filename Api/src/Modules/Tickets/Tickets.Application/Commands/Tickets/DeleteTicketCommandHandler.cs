using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.Commands.Tickets;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;
using System.Linq;

namespace Tickets.Application.Commands;

public class DeleteTicketCommandHandler : ICommandHandler<DeleteTicketCommand, TicketDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITicketRepository _repository;

    public DeleteTicketCommandHandler(IDateTimeProvider dateTimeProvider, ITicketRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<TicketDto> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var ticket = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (ticket is null)
            throw AppException.NotFound($"O ticket não existe!");

        if (!ticket.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O ticket foi modificado por outro usuário. Recarregue a página e tente novamente.");

        ticket.SetDeletedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(ticket);
        if (!success)
            throw AppException.BadRequest($"O ticket não foi inserido!");

        return TicketMappings.ToDto(ticket);
    }
}
