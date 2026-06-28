using BuildingBlocks.Application.Interfaces.Query;
using FeatureFlags.Application.DTOs;

namespace FeatureFlags.Application.Queries;

public record GetFeatureFlagByNameQuery(string Name) : IQuery<FeatureFlagDto>;

