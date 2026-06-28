using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.Team;

public class UpdateTeamCommandHandler : ICommandHandler<UpdateTeamCommand, TeamDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITeamRepository _repository;
    private readonly IRoleRepository _roleRepository;

    public UpdateTeamCommandHandler(
        IDateTimeProvider dateTimeProvider,
        ITeamRepository repository,
        IRoleRepository roleRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
        _roleRepository = roleRepository;
    }

    public async Task<TeamDto> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;
        var code = request.Code.Trim().ToUpperInvariant();

        await ValidateOrganizationRoleAsync(request.RoleId);

        var codeExists = await _repository.GetByCodeAsync(request.OrganizationId, code);
        if (codeExists != null && codeExists.Id != request.Id)
            throw AppException.Conflict($"A equipe com código {code} já existe!");

        var nameExists = await _repository.GetByNameAsync(request.OrganizationId, request.Name);
        if (nameExists != null && nameExists.Id != request.Id)
            throw AppException.Conflict($"A equipe {request.Name} já existe!");

        var team = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (team is null)
            throw AppException.NotFound($"A equipe não existe!");

        if (!team.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A equipe foi modificada por outro usuário. Recarregue a página e tente novamente.");

        if (string.Equals(team.Code, TeamEnum.OrganizationAdmin.Code, StringComparison.OrdinalIgnoreCase) && !string.Equals(code, TeamEnum.OrganizationAdmin.Code, StringComparison.OrdinalIgnoreCase))
            throw AppException.Conflict($"O código da equipe Organization Admin não pode ser alterado!");

        team.SetName(request.Name, dateNow);
        team.SetCode(code, dateNow);
        team.SetRoleId(request.RoleId, dateNow);

        var success = await _repository.UpdateAsync(team);
        if (!success)
            throw AppException.BadRequest($"A equipe {request.Name} não foi atualizada!");

        return team.ToDto();
    }

    private async Task ValidateOrganizationRoleAsync(Guid roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role is null || !string.Equals(role.Scope, "organization", StringComparison.OrdinalIgnoreCase))
            throw AppException.BadRequest($"O papel informado não é válido para equipes da organização!");
    }
}
