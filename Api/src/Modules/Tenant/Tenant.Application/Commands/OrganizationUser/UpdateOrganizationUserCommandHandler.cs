using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;
using BuildingBlocks.Application.Enums;

namespace Tenant.Application.Commands.OrganizationUser;

public class UpdateOrganizationUserCommandHandler : ICommandHandler<UpdateOrganizationUserCommand, OrganizationUserDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrganizationUserRepository _repository;
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;

    public UpdateOrganizationUserCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IOrganizationUserRepository repository,
        ITeamRepository teamRepository,
        IUserRepository userRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
        _teamRepository = teamRepository;
        _userRepository = userRepository;
    }

    public async Task<OrganizationUserDto> Handle(UpdateOrganizationUserCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var organizationUser = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (organizationUser is null)
            throw AppException.NotFound($"A relação entre organização e usuário não existe!");

        if (!organizationUser.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A relação entre organização e usuário foi modificada por outro usuário. Recarregue a página e tente novamente.");

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            throw AppException.NotFound($"O usuário não existe!");

        if (user.RoleId == RoleEnum.SuperAdmin.Id)
            throw AppException.BadRequest($"SuperAdmin não pode ser vinculado a equipe de organização!");

        var team = await _teamRepository.GetByIdAsync(request.OrganizationId, request.TeamId);
        if (team is null)
            throw AppException.NotFound($"A equipe não existe!");

        var organizationUserExists = await _repository.GetByUserIdAsync(request.OrganizationId, request.UserId);
        if (organizationUserExists != null && organizationUserExists.Id != request.Id)
            throw AppException.Conflict($"A relação entre organização e usuário já existe!");

        organizationUser.SetUserId(request.UserId, dateNow);
        organizationUser.SetTeamId(request.TeamId, dateNow);

        var success = await _repository.UpdateAsync(organizationUser);
        if (!success)
            throw AppException.BadRequest($"A relação entre organização e usuário não foi atualizada!");

        return OrganizationUserMappings.ToDto(organizationUser);
    }
}

