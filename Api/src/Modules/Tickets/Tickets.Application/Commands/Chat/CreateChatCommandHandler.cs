using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;

namespace Tickets.Application.Commands.Chat;

public class CreateChatCommandHandler : ICommandHandler<CreateChatCommand, ChatDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IChatRepository _repository;
    private readonly ITicketRepository _ticketRepository;

    public CreateChatCommandHandler(IDateTimeProvider dateTimeProvider, IChatRepository repository, ITicketRepository ticketRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
        _ticketRepository = ticketRepository;
    }

    public async Task<ChatDto> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var ticket = await _ticketRepository.GetByIdAsync(request.OrganizationId, request.TicketId);
        if (ticket is null)
            throw AppException.NotFound($"O ticket não existe!");

        var chat = new Domain.Entities.Chat(Guid.NewGuid(), dateNow);
        chat.SetTicketId(request.TicketId, dateNow);
        chat.SetUserId(request.UserId, dateNow);
        chat.SetMessage(request.Message, dateNow);

        var success = await _repository.CreateAsync(chat);
        if (!success)
            throw AppException.BadRequest($"O chat do ticket {request.TicketId} do usuário {request.UserId} não foi inserido!");

        return ChatMappings.ToDto(chat);
    }
}
