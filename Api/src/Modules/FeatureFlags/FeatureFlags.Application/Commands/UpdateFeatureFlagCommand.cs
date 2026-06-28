using BuildingBlocks.Application.Interfaces.Command;
using FeatureFlags.Application.DTOs;

namespace FeatureFlags.Application.Commands;

public record UpdateFeatureFlagCommand(Guid Id, string Name, string Description, bool Value, byte[] RowVersion) : ICommand<FeatureFlagDto>;
