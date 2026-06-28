using BuildingBlocks.Application.Queries;
using FeatureFlags.Application.DTOs;

namespace FeatureFlags.Application.Queries;

public record GetFeatureFlagByPageQuery(int PageIndex, int PageSize) : PageQuery<FeatureFlagDto>(PageIndex, PageSize);
