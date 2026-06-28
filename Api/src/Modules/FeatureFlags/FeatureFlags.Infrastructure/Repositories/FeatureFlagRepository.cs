using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using FeatureFlags.Application.Interfaces;
using FeatureFlags.Domain.Entities;
using System.Text;

namespace FeatureFlags.Infrastructure.Repositories;

public class FeatureFlagRepository : IFeatureFlagRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public FeatureFlagRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CreateAsync(FeatureFlag featureFlag)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.FeatureFlag.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, name, description, value)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @Name, @Description, @Value)");

        var parameters = new
        {
            featureFlag.Id,
            featureFlag.CreatedAt,
            featureFlag.UpdatedAt,
            featureFlag.Name,
            featureFlag.Description,
            featureFlag.Value
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(FeatureFlag featureFlag)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"DELETE FROM {DatabaseSchemaEnum.SdpDpNew.FeatureFlag.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            Id = featureFlag.Id,
            featureFlag.RowVersion,
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<FeatureFlag?> GetByIdAsync(Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(FeatureFlag.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(FeatureFlag.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(FeatureFlag.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(FeatureFlag.RowVersion)}");
        builder.AppendLine($"     , name AS {nameof(FeatureFlag.Name)}");
        builder.AppendLine($"     , description AS {nameof(FeatureFlag.Description)}");
        builder.AppendLine($"     , value AS {nameof(FeatureFlag.Value)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.FeatureFlag.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");

        var parameters = new { Id = id };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<FeatureFlag>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<FeatureFlag?> GetByNameAsync(string name)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(FeatureFlag.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(FeatureFlag.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(FeatureFlag.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(FeatureFlag.RowVersion)}");
        builder.AppendLine($"     , name AS {nameof(FeatureFlag.Name)}");
        builder.AppendLine($"     , description AS {nameof(FeatureFlag.Description)}");
        builder.AppendLine($"     , value AS {nameof(FeatureFlag.Value)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.FeatureFlag.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND name = @Name");

        var parameters = new { Name = name };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<FeatureFlag>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> UpdateAsync(FeatureFlag featureFlag)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.FeatureFlag.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , name = @Name");
        builder.AppendLine("  , description = @Description");
        builder.AppendLine("  , value = @Value");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            featureFlag.Id,
            featureFlag.UpdatedAt,
            featureFlag.RowVersion,
            featureFlag.Name,
            featureFlag.Description,
            featureFlag.Value
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
