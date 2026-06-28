using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.Team;

public class DeleteTeamCommandHandler : ICommandHandler<DeleteTeamCommand, TeamDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITeamRepository _repository;

    public DeleteTeamCommandHandler(IDateTimeProvider dateTimeProvider, ITeamRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<TeamDto> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var team = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (team is null)
            throw AppException.NotFound($"A equipe não existe!");

        if (!team.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A equipe foi modificada por outro usuário. Recarregue a página e tente novamente.");

        if (string.Equals(team.Code, TeamEnum.OrganizationAdmin.Code, StringComparison.OrdinalIgnoreCase))
            throw AppException.Conflict($"A equipe Organization Admin não pode ser removida!");

        var activeTeamCount = await _repository.CountActiveByOrganizationIdAsync(request.OrganizationId);
        if (activeTeamCount <= 1)
            throw AppException.Conflict($"A organização deve manter pelo menos uma equipe ativa!");

        team.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(team);
        if (!success)
            throw AppException.BadRequest($"A equipe não foi deletada!");

        return team.ToDto();
    }
}
