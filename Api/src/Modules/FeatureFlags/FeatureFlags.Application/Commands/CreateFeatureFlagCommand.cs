using BuildingBlocks.Application.Interfaces.Command;
using FeatureFlags.Application.DTOs;

namespace FeatureFlags.Application.Commands;

public record CreateFeatureFlagCommand(string Name, string Description) : ICommand<FeatureFlagDto>;
