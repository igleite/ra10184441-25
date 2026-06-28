using FeatureFlags.Domain.Entities;

namespace FeatureFlags.Application.Interfaces;

public interface IFeatureFlagRepository
{
    Task<FeatureFlag?> GetByIdAsync(Guid id);
    Task<FeatureFlag?> GetByNameAsync(string name);
    Task<bool> CreateAsync(FeatureFlag featureFlag);
    Task<bool> UpdateAsync(FeatureFlag featureFlag);
    Task<bool> DeleteAsync(FeatureFlag featureFlag);
}
