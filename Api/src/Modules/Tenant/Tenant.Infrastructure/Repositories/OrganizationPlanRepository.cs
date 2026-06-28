using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class OrganizationPlanRepository : IOrganizationPlanRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public OrganizationPlanRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrganizationPlan?> GetByIdAsync(Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(OrganizationPlan.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(OrganizationPlan.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(OrganizationPlan.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(OrganizationPlan.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(OrganizationPlan.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(OrganizationPlan.OrganizationId)}");
        builder.AppendLine($"     , plan_id AS {nameof(OrganizationPlan.PlanId)}");
        builder.AppendLine($"     , description AS {nameof(OrganizationPlan.Description)}");
        builder.AppendLine($"     , max_users AS {nameof(OrganizationPlan.MaxUsers)}");
        builder.AppendLine($"     , max_clients AS {nameof(OrganizationPlan.MaxClients)}");
        builder.AppendLine($"     , max_tickets AS {nameof(OrganizationPlan.MaxTickets)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.OrganizationPlan.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Id = id
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<OrganizationPlan>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<OrganizationPlan?> GetByOrganizationIdAsync(Guid organizationId)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(OrganizationPlan.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(OrganizationPlan.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(OrganizationPlan.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(OrganizationPlan.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(OrganizationPlan.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(OrganizationPlan.OrganizationId)}");
        builder.AppendLine($"     , plan_id AS {nameof(OrganizationPlan.PlanId)}");
        builder.AppendLine($"     , description AS {nameof(OrganizationPlan.Description)}");
        builder.AppendLine($"     , max_users AS {nameof(OrganizationPlan.MaxUsers)}");
        builder.AppendLine($"     , max_clients AS {nameof(OrganizationPlan.MaxClients)}");
        builder.AppendLine($"     , max_tickets AS {nameof(OrganizationPlan.MaxTickets)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.OrganizationPlan.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<OrganizationPlan>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(OrganizationPlan organizationPlan)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.OrganizationPlan.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, organization_id, plan_id, description, max_users, max_clients, max_tickets)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @OrganizationId, @PlanId, @Description, @MaxUsers, @MaxClients, @MaxTickets)");

        var parameters = new
        {
            organizationPlan.Id,
            organizationPlan.CreatedAt,
            organizationPlan.UpdatedAt,
            organizationPlan.InactivedAt,
            organizationPlan.OrganizationId,
            organizationPlan.PlanId,
            organizationPlan.Description,
            organizationPlan.MaxUsers,
            organizationPlan.MaxClients,
            organizationPlan.MaxTickets
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(OrganizationPlan organizationPlan)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.OrganizationPlan.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , plan_id = @PlanId");
        builder.AppendLine("  , description = @Description");
        builder.AppendLine("  , max_users = @MaxUsers");
        builder.AppendLine("  , max_clients = @MaxClients");
        builder.AppendLine("  , max_tickets = @MaxTickets");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            organizationPlan.Id,
            organizationPlan.UpdatedAt,
            organizationPlan.OrganizationId,
            organizationPlan.RowVersion,
            organizationPlan.PlanId,
            organizationPlan.Description,
            organizationPlan.MaxUsers,
            organizationPlan.MaxClients,
            organizationPlan.MaxTickets
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(OrganizationPlan organizationPlan)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.OrganizationPlan.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            organizationPlan.Id,
            organizationPlan.OrganizationId,
            organizationPlan.RowVersion,
            organizationPlan.InactivedAt,
            organizationPlan.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
