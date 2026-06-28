using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using FeatureFlags.Application.DTOs;
using FeatureFlags.Application.Interfaces;
using FeatureFlags.Application.Mappings;
using System.Linq;

namespace FeatureFlags.Application.Commands;

public class UpdateFeatureFlagCommandHandler : ICommandHandler<UpdateFeatureFlagCommand, FeatureFlagDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IFeatureFlagRepository _repository;

    public UpdateFeatureFlagCommandHandler(IDateTimeProvider dateTimeProvider, IFeatureFlagRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<FeatureFlagDto> Handle(UpdateFeatureFlagCommand request, CancellationToken cancellationToken)
    {
        var featureFlag = await _repository.GetByIdAsync(request.Id);
        if (featureFlag is null)
            throw AppException.NotFound($"A feature flag {request.Name} não existe!");

        if (!featureFlag.RowVersion.SequenceEqual(request.RowVersion))
            throw AppException.Conflict($"A feature flag {request.Name} foi modificada por outro usuário. Recarregue a página e tente novamente.");

        var dateNow = _dateTimeProvider.UtcNow;
        featureFlag.SetName(request.Name, dateNow);
        featureFlag.SetDescription(request.Description, dateNow);
        featureFlag.SetValue(request.Value, dateNow);

        var success = await _repository.UpdateAsync(featureFlag);
        if (!success)
            throw AppException.BadRequest($"A feature flag {request.Name} não foi atualizada!");

        return FeatureFlagMappings.ToDto(featureFlag);
    }
}
