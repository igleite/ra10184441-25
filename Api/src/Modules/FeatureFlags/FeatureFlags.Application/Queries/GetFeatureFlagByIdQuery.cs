using BuildingBlocks.Application.Interfaces.Query;
using FeatureFlags.Application.DTOs;

namespace FeatureFlags.Application.Queries;

public record GetFeatureFlagByIdQuery(Guid Id) : IQuery<FeatureFlagDto>;

