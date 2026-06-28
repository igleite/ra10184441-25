using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.OrganizationUser;

public class CreateOrganizationUserCommandHandler : ICommandHandler<CreateOrganizationUserCommand, OrganizationUserDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrganizationUserRepository _repository;
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;

    public CreateOrganizationUserCommandHandler(
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

    public async Task<OrganizationUserDto> Handle(CreateOrganizationUserCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            throw AppException.NotFound($"O usuário não existe!");

        if (user.RoleId == RoleEnum.SuperAdmin.Id)
            throw AppException.BadRequest($"SuperAdmin não pode ser vinculado a equipe de organização!");

        var team = await _teamRepository.GetByIdAsync(request.OrganizationId, request.TeamId);
        if (team is null)
            throw AppException.NotFound($"A equipe não existe!");

        var organizationUserExists = await _repository.GetByUserIdAsync(request.OrganizationId, request.UserId);
        if (organizationUserExists != null)
            throw AppException.Conflict($"A relação entre organização e usuário já existe!");

        var organizationUser = new Domain.Entities.OrganizationUser(Guid.NewGuid(), dateNow, request.OrganizationId);
        organizationUser.SetUserId(request.UserId, dateNow);
        organizationUser.SetTeamId(request.TeamId, dateNow);

        var success = await _repository.CreateAsync(organizationUser);
        if (!success)
            throw AppException.BadRequest($"A relação entre organização e usuário não foi inserida!");

        return OrganizationUserMappings.ToDto(organizationUser);
    }
}

