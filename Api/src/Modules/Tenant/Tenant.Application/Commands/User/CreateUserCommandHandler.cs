using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.User;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IDateTimeProvider dateTimeProvider, IUserRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var emailExists = await _repository.GetByEmailAsync(request.Email);
        if (emailExists != null)
            throw AppException.Conflict($"O usuário com email {request.Email} já existe!");

        var user = new Domain.Entities.User(Guid.NewGuid(), dateNow);
        user.SetName(request.Name, dateNow);
        user.SetEmail(request.Email, dateNow);
        user.SetRoleId(request.Role.Id, dateNow);

        var success = await _repository.CreateAsync(user);
        if (!success)
            throw AppException.BadRequest($"O usuário {request.Name} não foi inserido!");

        return UserMappings.ToDto(user);
    }
}

