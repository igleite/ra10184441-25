using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.Artifact;

public class DeleteArtifactCommandHandler : ICommandHandler<DeleteArtifactCommand, ArtifactDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IArtifactRepository _repository;

    public DeleteArtifactCommandHandler(IDateTimeProvider dateTimeProvider, IArtifactRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<ArtifactDto> Handle(DeleteArtifactCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;

        var artifact = await _repository.GetByIdAsync(request.OrganizationId, request.Id);
        if (artifact is null)
            throw AppException.NotFound($"O artefato não existe!");

        if (!artifact.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O artefato foi modificado por outro usuário. Recarregue a página e tente novamente.");

        artifact.SetInactivedAt(dateNow, dateNow);

        var success = await _repository.DeleteAsync(artifact);
        if (!success)
            throw AppException.BadRequest($"O artefato não foi deletado!");

        return artifact.ToDto();
    }
}
