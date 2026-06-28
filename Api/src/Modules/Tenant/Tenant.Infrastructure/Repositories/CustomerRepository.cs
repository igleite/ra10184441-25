using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Customer?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Customer.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Customer.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Customer.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Customer.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(Customer.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(Customer.OrganizationId)}");
        builder.AppendLine($"     , name AS {nameof(Customer.Name)}");
        builder.AppendLine($"     , document AS {nameof(Customer.Document)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Customer.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            OrganizationId = organizationId,
            Id = id,
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Customer>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<Customer?> GetByDocumentAsync(Guid organizationId, string document)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Customer.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Customer.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Customer.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Customer.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(Customer.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(Customer.OrganizationId)}");
        builder.AppendLine($"     , name AS {nameof(Customer.Name)}");
        builder.AppendLine($"     , document AS {nameof(Customer.Document)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Customer.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND document = @Document");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            OrganizationId = organizationId,
            Document = document
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Customer>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.Customer.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, organization_id, name, document)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @OrganizationId, @Name, @Document)");

        var parameters = new
        {
            customer.Id,
            customer.CreatedAt,
            customer.UpdatedAt,
            customer.InactivedAt,
            customer.OrganizationId,
            customer.Name,
            customer.Document
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Customer.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , name = @Name");
        builder.AppendLine("  , document = @Document");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            customer.Id,
            customer.UpdatedAt,
            customer.OrganizationId,
            customer.RowVersion,
            customer.Name,
            customer.Document
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Customer customer)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Customer.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            customer.Id,
            customer.OrganizationId,
            customer.RowVersion,
            customer.InactivedAt,
            customer.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
