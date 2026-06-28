using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using FeatureFlags.Application.DTOs;
using FeatureFlags.Application.Interfaces;
using FeatureFlags.Application.Mappings;
using System.Linq;

namespace FeatureFlags.Application.Commands;

public class DeleteFeatureFlagCommandHandler : ICommandHandler<DeleteFeatureFlagCommand, FeatureFlagDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IFeatureFlagRepository _repository;

    public DeleteFeatureFlagCommandHandler(IDateTimeProvider dateTimeProvider, IFeatureFlagRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<FeatureFlagDto> Handle(DeleteFeatureFlagCommand request, CancellationToken cancellationToken)
    {
        var featureFlag = await _repository.GetByIdAsync(request.Id);
        if (featureFlag is null)
            throw AppException.NotFound($"A feature flag não existe!");

        if (!featureFlag.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A feature flag foi modificada por outro usuário. Recarregue a página e tente novamente.");

        var success = await _repository.DeleteAsync(featureFlag);
        if (!success)
            throw AppException.BadRequest($"A feature flag {featureFlag.Name} não foi deletada!");

        return FeatureFlagMappings.ToDto(featureFlag);
    }
}
