using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.ArtifactType;

public class DeleteArtifactTypeCommandHandler : ICommandHandler<DeleteArtifactTypeCommand, ArtifactTypeDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IArtifactTypeRepository _repository;

    public DeleteArtifactTypeCommandHandler(IDateTimeProvider dateTimeProvider, IArtifactTypeRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<ArtifactTypeDto> Handle(DeleteArtifactTypeCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var artifactType = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (artifactType is null)
            throw AppException.NotFound($"O tipo de artefato não existe!");

        if (!artifactType.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O tipo de artefato foi modificado por outro usuário. Recarregue a página e tente novamente.");

        artifactType.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(artifactType);
        if (!success)
            throw AppException.BadRequest($"O tipo de artefato não foi deletado!");

        return artifactType.ToDto();
    }
}
