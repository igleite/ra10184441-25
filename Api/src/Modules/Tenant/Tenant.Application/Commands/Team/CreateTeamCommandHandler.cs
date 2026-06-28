using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.Team;

public class CreateTeamCommandHandler : ICommandHandler<CreateTeamCommand, TeamDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITeamRepository _repository;
    private readonly IRoleRepository _roleRepository;

    public CreateTeamCommandHandler(
        IDateTimeProvider dateTimeProvider,
        ITeamRepository repository,
        IRoleRepository roleRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
        _roleRepository = roleRepository;
    }

    public async Task<TeamDto> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;
        var code = request.Code.Trim().ToUpperInvariant();

        await ValidateOrganizationRoleAsync(request.RoleId);

        var codeExists = await _repository.GetByCodeAsync(request.OrganizationId, code);
        if (codeExists != null)
            throw AppException.Conflict($"A equipe com código {code} já existe!");

        var nameExists = await _repository.GetByNameAsync(request.OrganizationId, request.Name);
        if (nameExists != null)
            throw AppException.Conflict($"A equipe {request.Name} já existe!");

        var team = new Domain.Entities.Team(Guid.NewGuid(), dateNow, request.OrganizationId);
        team.SetName(request.Name, dateNow);
        team.SetCode(code, dateNow);
        team.SetRoleId(request.RoleId, dateNow);

        var success = await _repository.CreateAsync(team);
        if (!success)
            throw AppException.BadRequest($"A equipe {request.Name} não foi inserida!");

        return team.ToDto();
    }

    private async Task ValidateOrganizationRoleAsync(Guid roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role is null || !string.Equals(role.Scope, "organization", StringComparison.OrdinalIgnoreCase))
            throw AppException.BadRequest($"O papel informado não é válido para equipes da organização!");
    }
}
