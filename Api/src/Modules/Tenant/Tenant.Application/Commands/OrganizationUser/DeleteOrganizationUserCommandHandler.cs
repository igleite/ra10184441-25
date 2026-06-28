using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.OrganizationUser;

public class DeleteOrganizationUserCommandHandler : ICommandHandler<DeleteOrganizationUserCommand, OrganizationUserDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrganizationUserRepository _repository;

    public DeleteOrganizationUserCommandHandler(IDateTimeProvider dateTimeProvider, IOrganizationUserRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<OrganizationUserDto> Handle(DeleteOrganizationUserCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var organizationUser = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (organizationUser is null)
            throw AppException.NotFound($"A relação entre organização e usuário não existe!");

        if (!organizationUser.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A relação entre organização e usuário foi modificada por outro usuário. Recarregue a página e tente novamente.");

        organizationUser.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(organizationUser);
        if (!success)
            throw AppException.BadRequest($"A relação entre organização e usuário não foi deletada!");

        return OrganizationUserMappings.ToDto(organizationUser);
    }
}

