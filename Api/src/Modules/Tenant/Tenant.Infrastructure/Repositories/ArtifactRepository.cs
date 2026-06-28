using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class ArtifactRepository : IArtifactRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ArtifactRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Artifact?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       a.id AS {nameof(Artifact.Id)}");
        builder.AppendLine($"     , a.created_at AS {nameof(Artifact.CreatedAt)}");
        builder.AppendLine($"     , a.updated_at AS {nameof(Artifact.UpdatedAt)}");
        builder.AppendLine($"     , a.row_version AS {nameof(Artifact.RowVersion)}");
        builder.AppendLine($"     , a.inactived_at AS {nameof(Artifact.InactivedAt)}");
        builder.AppendLine($"     , a.artifact_type_id AS {nameof(Artifact.ArtifactTypeId)}");
        builder.AppendLine($"     , a.name AS {nameof(Artifact.Name)}");
        builder.AppendLine($"     , a.code AS {nameof(Artifact.Code)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName} a");
        builder.AppendLine($"INNER JOIN {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName} at ON at.id = a.artifact_type_id");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND a.id = @Id");
        builder.AppendLine("  AND at.organization_id = @OrganizationId");
        builder.AppendLine("  AND a.inactived_at IS NULL");
        builder.AppendLine("  AND at.inactived_at IS NULL");

        var parameters = new
        {
            Id = id,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Artifact>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<Artifact?> GetByArtifactTypeIdAndNameAsync(Guid organizationId, Guid artifactTypeId, string name)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       a.id AS {nameof(Artifact.Id)}");
        builder.AppendLine($"     , a.created_at AS {nameof(Artifact.CreatedAt)}");
        builder.AppendLine($"     , a.updated_at AS {nameof(Artifact.UpdatedAt)}");
        builder.AppendLine($"     , a.row_version AS {nameof(Artifact.RowVersion)}");
        builder.AppendLine($"     , a.inactived_at AS {nameof(Artifact.InactivedAt)}");
        builder.AppendLine($"     , a.artifact_type_id AS {nameof(Artifact.ArtifactTypeId)}");
        builder.AppendLine($"     , a.name AS {nameof(Artifact.Name)}");
        builder.AppendLine($"     , a.code AS {nameof(Artifact.Code)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName} a");
        builder.AppendLine($"INNER JOIN {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName} at ON at.id = a.artifact_type_id");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND a.name = @Name");
        builder.AppendLine("  AND a.artifact_type_id = @ArtifactTypeId");
        builder.AppendLine("  AND at.organization_id = @OrganizationId");
        builder.AppendLine("  AND a.inactived_at IS NULL");
        builder.AppendLine("  AND at.inactived_at IS NULL");

        var parameters = new
        {
            Name = name,
            ArtifactTypeId = artifactTypeId,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Artifact>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<Artifact?> GetByArtifactTypeIdAndCodeAsync(Guid organizationId, Guid artifactTypeId, string code)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       a.id AS {nameof(Artifact.Id)}");
        builder.AppendLine($"     , a.created_at AS {nameof(Artifact.CreatedAt)}");
        builder.AppendLine($"     , a.updated_at AS {nameof(Artifact.UpdatedAt)}");
        builder.AppendLine($"     , a.row_version AS {nameof(Artifact.RowVersion)}");
        builder.AppendLine($"     , a.inactived_at AS {nameof(Artifact.InactivedAt)}");
        builder.AppendLine($"     , a.artifact_type_id AS {nameof(Artifact.ArtifactTypeId)}");
        builder.AppendLine($"     , a.name AS {nameof(Artifact.Name)}");
        builder.AppendLine($"     , a.code AS {nameof(Artifact.Code)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName} a");
        builder.AppendLine($"INNER JOIN {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName} at ON at.id = a.artifact_type_id");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND a.code = @Code");
        builder.AppendLine("  AND a.artifact_type_id = @ArtifactTypeId");
        builder.AppendLine("  AND at.organization_id = @OrganizationId");
        builder.AppendLine("  AND a.inactived_at IS NULL");
        builder.AppendLine("  AND at.inactived_at IS NULL");

        var parameters = new
        {
            Code = code,
            ArtifactTypeId = artifactTypeId,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Artifact>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(Artifact artifact)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, artifact_type_id, name, code)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @ArtifactTypeId, @Name, @Code)");

        var parameters = new
        {
            artifact.Id,
            artifact.CreatedAt,
            artifact.UpdatedAt,
            artifact.InactivedAt,
            artifact.ArtifactTypeId,
            artifact.Name,
            artifact.Code
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Artifact artifact)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , artifact_type_id = @ArtifactTypeId");
        builder.AppendLine("  , name = @Name");
        builder.AppendLine("  , code = @Code");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            artifact.Id,
            artifact.UpdatedAt,
            artifact.RowVersion,
            artifact.ArtifactTypeId,
            artifact.Name,
            artifact.Code
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Artifact artifact)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            artifact.Id,
            artifact.RowVersion,
            artifact.InactivedAt,
            artifact.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
