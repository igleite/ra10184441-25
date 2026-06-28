using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;
using System.Linq;

namespace Tickets.Application.Commands.Chat;

public class UpdateChatCommandHandler : ICommandHandler<UpdateChatCommand, ChatDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IChatRepository _repository;

    public UpdateChatCommandHandler(IDateTimeProvider dateTimeProvider, IChatRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<ChatDto> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var chat = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (chat is null || chat.TicketId != request.TicketId)
            throw AppException.NotFound($"O chat do ticket {request.TicketId} não existe!");

        if (!chat.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O chat foi modificado por outro usuário. Recarregue a página e tente novamente.");

        chat.SetTicketId(request.TicketId, dateNow);
        chat.SetMessage(request.Message, dateNow);

        var success = await _repository.UpdateAsync(chat);
        if (!success)
            throw AppException.BadRequest($"O chat do ticket {request.TicketId} não foi atualizado!");

        return ChatMappings.ToDto(chat);
    }
}
