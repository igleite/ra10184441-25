using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.Organization;

public class DeleteOrganizationCommandHandler : ICommandHandler<DeleteOrganizationCommand, OrganizationDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrganizationRepository _repository;

    public DeleteOrganizationCommandHandler(IDateTimeProvider dateTimeProvider, IOrganizationRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<OrganizationDto> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var organization = await _repository.GetByIdAsync(request.Id);
        if (organization is null)
            throw AppException.NotFound($"A organização não existe!");

        if (!organization.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A organização foi modificada por outro usuário. Recarregue a página e tente novamente.");

        var success = await _repository.DeleteAsync(organization);
        if (!success)
            throw AppException.BadRequest($"A organização não foi deletado!");

        return OrganizationMappings.ToDto(organization);
    }
}

