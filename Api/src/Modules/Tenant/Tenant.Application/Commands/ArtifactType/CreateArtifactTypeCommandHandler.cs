using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.ArtifactType;

public class CreateArtifactTypeCommandHandler : ICommandHandler<CreateArtifactTypeCommand, ArtifactTypeDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IArtifactTypeRepository _repository;

    public CreateArtifactTypeCommandHandler(IDateTimeProvider dateTimeProvider, IArtifactTypeRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<ArtifactTypeDto> Handle(CreateArtifactTypeCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var exists = await _repository.GetByNameAsync(request.OrganizationId, request.Name);
        if (exists != null)
            throw AppException.Conflict($"O tipo de artefato {request.Name} já existe!");

        var artifactType = new Domain.Entities.ArtifactType(Guid.NewGuid(), dateNow, request.OrganizationId);
        artifactType.SetName(request.Name, dateNow);

        var success = await _repository.CreateAsync(artifactType);
        if (!success)
            throw AppException.BadRequest($"O tipo de artefato {request.Name} não foi inserido!");

        return artifactType.ToDto();
    }
}
