using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class CustomerUserRepository : ICustomerUserRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerUserRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerUser?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(CustomerUser.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(CustomerUser.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(CustomerUser.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(CustomerUser.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(CustomerUser.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(CustomerUser.OrganizationId)}");
        builder.AppendLine($"     , customer_id AS {nameof(CustomerUser.CustomerId)}");
        builder.AppendLine($"     , user_id AS {nameof(CustomerUser.UserId)}");
        builder.AppendLine($"     , role_id AS {nameof(CustomerUser.RoleId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.CustomerUser.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Id = id,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<CustomerUser>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<CustomerUser?> GetByUserIdAsync(Guid organizationId, Guid userId)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT TOP 1");
        builder.AppendLine($"       id AS {nameof(CustomerUser.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(CustomerUser.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(CustomerUser.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(CustomerUser.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(CustomerUser.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(CustomerUser.OrganizationId)}");
        builder.AppendLine($"     , customer_id AS {nameof(CustomerUser.CustomerId)}");
        builder.AppendLine($"     , user_id AS {nameof(CustomerUser.UserId)}");
        builder.AppendLine($"     , role_id AS {nameof(CustomerUser.RoleId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.CustomerUser.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND user_id = @UserId");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            OrganizationId = organizationId,
            UserId = userId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<CustomerUser>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<CustomerUser?> GetByCustomerIdAndUserIdAsync(Guid organizationId, Guid customerId, Guid userId)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(CustomerUser.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(CustomerUser.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(CustomerUser.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(CustomerUser.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(CustomerUser.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(CustomerUser.OrganizationId)}");
        builder.AppendLine($"     , customer_id AS {nameof(CustomerUser.CustomerId)}");
        builder.AppendLine($"     , user_id AS {nameof(CustomerUser.UserId)}");
        builder.AppendLine($"     , role_id AS {nameof(CustomerUser.RoleId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.CustomerUser.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND customer_id = @CustomerId");
        builder.AppendLine("  AND user_id = @UserId");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            OrganizationId = organizationId,
            CustomerId = customerId,
            UserId = userId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<CustomerUser>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(CustomerUser customerUser)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.CustomerUser.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, organization_id, customer_id, user_id, role_id)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @OrganizationId, @CustomerId, @UserId, @RoleId)");

        var parameters = new
        {
            customerUser.Id,
            customerUser.CreatedAt,
            customerUser.UpdatedAt,
            customerUser.InactivedAt,
            customerUser.OrganizationId,
            customerUser.CustomerId,
            customerUser.UserId,
            customerUser.RoleId
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(CustomerUser customerUser)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.CustomerUser.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , customer_id = @CustomerId");
        builder.AppendLine("  , user_id = @UserId");
        builder.AppendLine("  , role_id = @RoleId");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            customerUser.Id,
            customerUser.UpdatedAt,
            customerUser.OrganizationId,
            customerUser.RowVersion,
            customerUser.CustomerId,
            customerUser.UserId,
            customerUser.RoleId
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(CustomerUser customerUser)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.CustomerUser.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            customerUser.Id,
            customerUser.OrganizationId,
            customerUser.RowVersion,
            customerUser.InactivedAt,
            customerUser.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
