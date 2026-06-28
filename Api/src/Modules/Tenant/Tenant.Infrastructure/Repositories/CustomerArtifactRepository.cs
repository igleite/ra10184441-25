using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class CustomerArtifactRepository : ICustomerArtifactRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerArtifactRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerArtifact?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       ca.id AS {nameof(CustomerArtifact.Id)}");
        builder.AppendLine($"     , ca.created_at AS {nameof(CustomerArtifact.CreatedAt)}");
        builder.AppendLine($"     , ca.updated_at AS {nameof(CustomerArtifact.UpdatedAt)}");
        builder.AppendLine($"     , ca.row_version AS {nameof(CustomerArtifact.RowVersion)}");
        builder.AppendLine($"     , ca.inactived_at AS {nameof(CustomerArtifact.InactivedAt)}");
        builder.AppendLine($"     , ca.customer_id AS {nameof(CustomerArtifact.CustomerId)}");
        builder.AppendLine($"     , ca.artifact_id AS {nameof(CustomerArtifact.ArtifactId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.CustomerArtifact.FullName} ca");
        builder.AppendLine($"INNER JOIN {DatabaseSchemaEnum.SdpDpNew.Customer.FullName} c ON c.id = ca.customer_id");
        builder.AppendLine($"INNER JOIN {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName} a ON a.id = ca.artifact_id");
        builder.AppendLine($"INNER JOIN {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName} at ON at.id = a.artifact_type_id");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND ca.id = @Id");
        builder.AppendLine("  AND c.organization_id = @OrganizationId");
        builder.AppendLine("  AND at.organization_id = @OrganizationId");
        builder.AppendLine("  AND ca.inactived_at IS NULL");
        builder.AppendLine("  AND c.inactived_at IS NULL");
        builder.AppendLine("  AND a.inactived_at IS NULL");
        builder.AppendLine("  AND at.inactived_at IS NULL");

        var parameters = new
        {
            Id = id,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<CustomerArtifact>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<CustomerArtifact?> GetByCustomerIdAndArtifactIdAsync(Guid organizationId, Guid customerId, Guid artifactId)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       ca.id AS {nameof(CustomerArtifact.Id)}");
        builder.AppendLine($"     , ca.created_at AS {nameof(CustomerArtifact.CreatedAt)}");
        builder.AppendLine($"     , ca.updated_at AS {nameof(CustomerArtifact.UpdatedAt)}");
        builder.AppendLine($"     , ca.row_version AS {nameof(CustomerArtifact.RowVersion)}");
        builder.AppendLine($"     , ca.inactived_at AS {nameof(CustomerArtifact.InactivedAt)}");
        builder.AppendLine($"     , ca.customer_id AS {nameof(CustomerArtifact.CustomerId)}");
        builder.AppendLine($"     , ca.artifact_id AS {nameof(CustomerArtifact.ArtifactId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.CustomerArtifact.FullName} ca");
        builder.AppendLine($"INNER JOIN {DatabaseSchemaEnum.SdpDpNew.Customer.FullName} c ON c.id = ca.customer_id");
        builder.AppendLine($"INNER JOIN {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName} a ON a.id = ca.artifact_id");
        builder.AppendLine($"INNER JOIN {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName} at ON at.id = a.artifact_type_id");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND ca.customer_id = @CustomerId");
        builder.AppendLine("  AND ca.artifact_id = @ArtifactId");
        builder.AppendLine("  AND c.organization_id = @OrganizationId");
        builder.AppendLine("  AND at.organization_id = @OrganizationId");
        builder.AppendLine("  AND ca.inactived_at IS NULL");
        builder.AppendLine("  AND c.inactived_at IS NULL");
        builder.AppendLine("  AND a.inactived_at IS NULL");
        builder.AppendLine("  AND at.inactived_at IS NULL");

        var parameters = new
        {
            CustomerId = customerId,
            ArtifactId = artifactId,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<CustomerArtifact>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(CustomerArtifact customerArtifact)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.CustomerArtifact.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, customer_id, artifact_id)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @CustomerId, @ArtifactId)");

        var parameters = new
        {
            customerArtifact.Id,
            customerArtifact.CreatedAt,
            customerArtifact.UpdatedAt,
            customerArtifact.InactivedAt,
            customerArtifact.CustomerId,
            customerArtifact.ArtifactId
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(CustomerArtifact customerArtifact)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.CustomerArtifact.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , customer_id = @CustomerId");
        builder.AppendLine("  , artifact_id = @ArtifactId");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            customerArtifact.Id,
            customerArtifact.UpdatedAt,
            customerArtifact.RowVersion,
            customerArtifact.CustomerId,
            customerArtifact.ArtifactId
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(CustomerArtifact customerArtifact)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.CustomerArtifact.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            customerArtifact.Id,
            customerArtifact.RowVersion,
            customerArtifact.InactivedAt,
            customerArtifact.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
