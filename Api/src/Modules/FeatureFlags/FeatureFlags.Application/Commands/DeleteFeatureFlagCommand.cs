using BuildingBlocks.Application.Interfaces.Command;
using FeatureFlags.Application.DTOs;

namespace FeatureFlags.Application.Commands;

public record DeleteFeatureFlagCommand(Guid Id, byte[] RowVersion) : ICommand<FeatureFlagDto>;
