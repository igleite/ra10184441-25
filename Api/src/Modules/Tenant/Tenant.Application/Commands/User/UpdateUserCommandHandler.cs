using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using System.Linq;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.User;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserRepository _repository;

    public UpdateUserCommandHandler(IDateTimeProvider dateTimeProvider, IUserRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var user = await _repository.GetByIdAsync(request.Id);
        if (user is null)
            throw AppException.NotFound($"O usuário não existe!");

        if (!user.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O usuário foi modificado por outro usuário. Recarregue a página e tente novamente.");

        var emailExists = await _repository.GetByEmailAsync(request.Email);
        if (emailExists != null && emailExists.Id != request.Id)
            throw AppException.Conflict($"O usuário com email {request.Email} já existe!");

        user.SetName(request.Name, dateNow);
        user.SetEmail(request.Email, dateNow);

        if (request.Role == RoleEnum.Nullable)
            user.SetRoleId(null, dateNow);
        else if (request.Role != RoleEnum.Unchanged)
            user.SetRoleId(request.Role.Id, dateNow);

        var success = await _repository.UpdateAsync(user);
        if (!success)
            throw AppException.BadRequest($"O usuário não foi atualizado!");

        return UserMappings.ToDto(user);
    }
}

