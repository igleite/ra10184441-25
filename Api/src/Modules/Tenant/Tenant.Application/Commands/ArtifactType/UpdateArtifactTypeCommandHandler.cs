using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.ArtifactType;

public class UpdateArtifactTypeCommandHandler : ICommandHandler<UpdateArtifactTypeCommand, ArtifactTypeDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IArtifactTypeRepository _repository;

    public UpdateArtifactTypeCommandHandler(IDateTimeProvider dateTimeProvider, IArtifactTypeRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<ArtifactTypeDto> Handle(UpdateArtifactTypeCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var nameExists = await _repository.GetByNameAsync(request.OrganizationId, request.Name);
        if (nameExists != null && nameExists.Id != request.Id)
            throw AppException.Conflict($"O tipo de artefato {request.Name} já existe!");

        var artifactType = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (artifactType is null)
            throw AppException.NotFound($"O tipo de artefato não existe!");

        if (!artifactType.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O tipo de artefato foi modificado por outro usuário. Recarregue a página e tente novamente.");

        artifactType.SetName(request.Name, dateNow);

        var success = await _repository.UpdateAsync(artifactType);
        if (!success)
            throw AppException.BadRequest($"O tipo de artefato {request.Name} não foi atualizado!");

        return artifactType.ToDto();
    }
}
