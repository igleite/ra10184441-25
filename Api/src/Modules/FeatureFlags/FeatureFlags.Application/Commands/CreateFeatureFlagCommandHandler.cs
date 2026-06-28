using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using FeatureFlags.Application.DTOs;
using FeatureFlags.Application.Interfaces;
using FeatureFlags.Application.Mappings;
using FeatureFlags.Domain.Entities;

namespace FeatureFlags.Application.Commands;

public class CreateFeatureFlagCommandHandler : ICommandHandler<CreateFeatureFlagCommand, FeatureFlagDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IFeatureFlagRepository _repository;

    public CreateFeatureFlagCommandHandler(IDateTimeProvider dateTimeProvider, IFeatureFlagRepository repository)
    {
        _dateTimeProvider = dateTimeProvider;
        _repository = repository;
    }

    public async Task<FeatureFlagDto> Handle(CreateFeatureFlagCommand request, CancellationToken cancellationToken)
    {
        var flagExiste = await _repository.GetByNameAsync(request.Name);
        if (flagExiste != null)
            throw AppException.Conflict($"A feature flag {request.Name} já existe!");

        var dateNow = _dateTimeProvider.UtcNow;
        var featureFlag = new FeatureFlag(Guid.NewGuid(), dateNow);
        featureFlag.SetName(request.Name, dateNow);
        featureFlag.SetDescription(request.Description, dateNow);
        featureFlag.SetValue(false, dateNow);

        var success = await _repository.CreateAsync(featureFlag);
        if (!success)
            throw AppException.BadRequest($"A feature flag {request.Name} não foi inserida!");

        return FeatureFlagMappings.ToDto(featureFlag);
    }
}
