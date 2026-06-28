using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;
using Tenant.Application.Interfaces;
using Tenant.Application.Mappings;

namespace Tenant.Application.Commands.Artifact;

public class UpdateArtifactCommandHandler : ICommandHandler<UpdateArtifactCommand, ArtifactDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IArtifactRepository _artifactRepository;
    private readonly IArtifactTypeRepository _artifactTypeRepository;

    public UpdateArtifactCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IArtifactRepository artifactRepository,
        IArtifactTypeRepository artifactTypeRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _artifactRepository = artifactRepository;
        _artifactTypeRepository = artifactTypeRepository;
    }

    public async Task<ArtifactDto> Handle(UpdateArtifactCommand request, CancellationToken cancellationToken)
    {
        var dateNow = _dateTimeProvider.UtcNow;
        var code = request.Code.Trim().ToUpperInvariant();

        var artifactType = await _artifactTypeRepository.GetByIdAsync(request.OrganizationId, request.ArtifactTypeId);
        if (artifactType is null)
            throw AppException.NotFound($"O tipo de artefato não existe!");

        var codeExists = await _artifactRepository.GetByArtifactTypeIdAndCodeAsync(request.OrganizationId, request.ArtifactTypeId, code);
        if (codeExists != null && codeExists.Id != request.Id)
            throw AppException.Conflict($"O artefato com código {code} já existe para este tipo!");

        var nameExists = await _artifactRepository.GetByArtifactTypeIdAndNameAsync(request.OrganizationId, request.ArtifactTypeId, request.Name);
        if (nameExists != null && nameExists.Id != request.Id)
            throw AppException.Conflict($"O artefato {request.Name} já existe para este tipo!");

        var artifact = await _artifactRepository.GetByIdAsync(request.OrganizationId, request.Id);
        if (artifact is null)
            throw AppException.NotFound($"O artefato não existe!");

        if (!artifact.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"O artefato foi modificado por outro usuário. Recarregue a página e tente novamente.");

        artifact.SetArtifactTypeId(request.ArtifactTypeId, dateNow);
        artifact.SetName(request.Name, dateNow);
        artifact.SetCode(code, dateNow);

        var success = await _artifactRepository.UpdateAsync(artifact);
        if (!success)
            throw AppException.BadRequest($"O artefato {request.Name} não foi atualizado!");

        return artifact.ToDto();
    }
}
