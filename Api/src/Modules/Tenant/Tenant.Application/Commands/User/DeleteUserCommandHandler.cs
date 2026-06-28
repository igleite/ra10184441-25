using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.User;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, UserDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserRepository _repository;

    public DeleteUserCommandHandler(IDateTimeProvider dateTimeProvider, IUserRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<UserDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var user = await _repository.GetByIdAsync(request.Id);
        if (user is null)
            throw AppException.NotFound($"O usuário não existe!");

        if (!user.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O usuário foi modificado por outro usuário. Recarregue a página e tente novamente.");

        user.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(user);
        if (!success)
            throw AppException.BadRequest($"O usuário não foi deletado!");

        return UserMappings.ToDto(user);
    }
}

