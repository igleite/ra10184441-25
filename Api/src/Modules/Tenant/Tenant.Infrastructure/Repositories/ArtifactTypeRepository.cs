using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class ArtifactTypeRepository : IArtifactTypeRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ArtifactTypeRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ArtifactType?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(ArtifactType.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(ArtifactType.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(ArtifactType.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(ArtifactType.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(ArtifactType.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(ArtifactType.OrganizationId)}");
        builder.AppendLine($"     , name AS {nameof(ArtifactType.Name)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Id = id,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<ArtifactType>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<ArtifactType?> GetByNameAsync(Guid organizationId, string name)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(ArtifactType.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(ArtifactType.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(ArtifactType.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(ArtifactType.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(ArtifactType.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(ArtifactType.OrganizationId)}");
        builder.AppendLine($"     , name AS {nameof(ArtifactType.Name)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND name = @Name");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Name = name,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<ArtifactType>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(ArtifactType artifactType)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, organization_id, name)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @OrganizationId, @Name)");

        var parameters = new
        {
            artifactType.Id,
            artifactType.CreatedAt,
            artifactType.UpdatedAt,
            artifactType.InactivedAt,
            artifactType.OrganizationId,
            artifactType.Name
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(ArtifactType artifactType)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , name = @Name");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            artifactType.Id,
            artifactType.UpdatedAt,
            artifactType.OrganizationId,
            artifactType.RowVersion,
            artifactType.Name
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(ArtifactType artifactType)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            artifactType.Id,
            artifactType.OrganizationId,
            artifactType.RowVersion,
            artifactType.InactivedAt,
            artifactType.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
