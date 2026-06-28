using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class OrganizationUserRepository : IOrganizationUserRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public OrganizationUserRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrganizationUser?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(OrganizationUser.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(OrganizationUser.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(OrganizationUser.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(OrganizationUser.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(OrganizationUser.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(OrganizationUser.OrganizationId)}");
        builder.AppendLine($"     , user_id AS {nameof(OrganizationUser.UserId)}");
        builder.AppendLine($"     , team_id AS {nameof(OrganizationUser.TeamId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.OrganizationUser.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Id = id,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<OrganizationUser>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<OrganizationUser?> GetByUserIdAsync(Guid organizationId, Guid userId)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(OrganizationUser.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(OrganizationUser.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(OrganizationUser.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(OrganizationUser.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(OrganizationUser.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(OrganizationUser.OrganizationId)}");
        builder.AppendLine($"     , user_id AS {nameof(OrganizationUser.UserId)}");
        builder.AppendLine($"     , team_id AS {nameof(OrganizationUser.TeamId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.OrganizationUser.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND user_id = @UserId");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            OrganizationId = organizationId,
            UserId = userId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<OrganizationUser>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(OrganizationUser organizationUser)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.OrganizationUser.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, organization_id, user_id, team_id)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @OrganizationId, @UserId, @TeamId)");

        var parameters = new
        {
            organizationUser.Id,
            organizationUser.CreatedAt,
            organizationUser.UpdatedAt,
            organizationUser.InactivedAt,
            organizationUser.OrganizationId,
            organizationUser.UserId,
            organizationUser.TeamId
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(OrganizationUser organizationUser)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.OrganizationUser.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , user_id = @UserId");
        builder.AppendLine("  , team_id = @TeamId");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            organizationUser.Id,
            organizationUser.UpdatedAt,
            organizationUser.OrganizationId,
            organizationUser.RowVersion,
            organizationUser.UserId,
            organizationUser.TeamId
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(OrganizationUser organizationUser)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.OrganizationUser.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            organizationUser.Id,
            organizationUser.OrganizationId,
            organizationUser.RowVersion,
            organizationUser.InactivedAt,
            organizationUser.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}

