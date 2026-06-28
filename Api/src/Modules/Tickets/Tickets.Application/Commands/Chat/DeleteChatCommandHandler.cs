using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tickets.Application.DTOs;
using Tickets.Application.Interfaces;
using Tickets.Application.Mappings;
using System.Linq;

namespace Tickets.Application.Commands.Chat;

public class DeleteChatCommandHandler : ICommandHandler<DeleteChatCommand, ChatDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IChatRepository _repository;

    public DeleteChatCommandHandler(IDateTimeProvider dateTimeProvider, IChatRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<ChatDto> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var chat = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (chat is null)
            throw AppException.NotFound($"O chat não existe!");

        if (!chat.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O chat foi modificado por outro usuário. Recarregue a página e tente novamente.");

        chat.SetDeletedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(chat);
        if (!success)
            throw AppException.BadRequest($"O chat não foi deletado!");

        return ChatMappings.ToDto(chat);
    }
}

