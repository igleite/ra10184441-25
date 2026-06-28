using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;
using System.Linq;

namespace Tenant.Application.Commands.Organization;

public class UpdateOrganizationCommandHandler : ICommandHandler<UpdateOrganizationCommand, OrganizationDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrganizationRepository _repository;

    public UpdateOrganizationCommandHandler(IDateTimeProvider dateTimeProvider, IOrganizationRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<OrganizationDto> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var organization = await _repository.GetByIdAsync(request.Id);
        if (organization is null)
            throw AppException.NotFound($"A organização não existe!");

        if (!organization.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A organização foi modificada por outro usuário. Recarregue a página e tente novamente.");

        var documentExists = await _repository.GetByDocumentAsync(request.Document);
        if (documentExists != null && documentExists.Id != request.Id)
            throw AppException.Conflict($"A organização já existe!");

        var slugExists = await _repository.GetBySlugAsync(request.Slug);
        if (slugExists != null)
            throw AppException.Conflict($"O slug já está sendo utilizado!");

        organization.SetName(request.Name, dateNow);
        organization.SetDocument(request.Document, dateNow);
        organization.SetSlug(request.Slug, dateNow);

        var success = await _repository.UpdateAsync(organization);
        if (!success)
            throw AppException.BadRequest($"A organização não foi atualizado!");

        return OrganizationMappings.ToDto(organization);
    }
}

