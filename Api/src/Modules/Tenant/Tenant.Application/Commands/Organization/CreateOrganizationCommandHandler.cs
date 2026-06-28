using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.Organization;

public class CreateOrganizationCommandHandler : ICommandHandler<CreateOrganizationCommand, OrganizationDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrganizationRepository _repository;
    private readonly ITeamRepository _teamRepository;

    public CreateOrganizationCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IOrganizationRepository repository,
        ITeamRepository teamRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
        _teamRepository = teamRepository;
    }

    public async Task<OrganizationDto> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var documentExists = await _repository.GetByDocumentAsync(request.Document);
        if (documentExists != null)
            throw AppException.Conflict($"A organização já existe!");

        var slugExists = await _repository.GetBySlugAsync(request.Slug);
        if (slugExists != null)
            throw AppException.Conflict($"O slug já está sendo utilizado!");

        var organization = new Domain.Entities.Organization(Guid.NewGuid(), dateNow);
        organization.SetName(request.Name, updatedAt: dateNow);
        organization.SetDocument(request.Document, dateNow);
        organization.SetSlug(request.Slug, dateNow);

        var success = await _repository.CreateAsync(organization);
        if (!success)
            throw AppException.BadRequest($"A organização  não foi inserido!");

        return OrganizationMappings.ToDto(organization);
    }
}
