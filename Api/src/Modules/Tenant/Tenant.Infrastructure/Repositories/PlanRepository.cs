using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public PlanRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Plan?> GetByIdAsync(Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Plan.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Plan.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Plan.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Plan.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(Plan.InactivedAt)}");
        builder.AppendLine($"     , name AS {nameof(Plan.Name)}");
        builder.AppendLine($"     , description AS {nameof(Plan.Description)}");
        builder.AppendLine($"     , max_users AS {nameof(Plan.MaxUsers)}");
        builder.AppendLine($"     , max_clients AS {nameof(Plan.MaxClients)}");
        builder.AppendLine($"     , max_tickets AS {nameof(Plan.MaxTickets)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Plan.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Id = id
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Plan>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<Plan?> GetByNameAsync(string name)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Plan.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Plan.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Plan.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Plan.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(Plan.InactivedAt)}");
        builder.AppendLine($"     , name AS {nameof(Plan.Name)}");
        builder.AppendLine($"     , description AS {nameof(Plan.Description)}");
        builder.AppendLine($"     , max_users AS {nameof(Plan.MaxUsers)}");
        builder.AppendLine($"     , max_clients AS {nameof(Plan.MaxClients)}");
        builder.AppendLine($"     , max_tickets AS {nameof(Plan.MaxTickets)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Plan.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND name = @Name");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Name = name
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Plan>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(Plan plan)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.Plan.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, name, description, max_users, max_clients, max_tickets)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @Name, @Description, @MaxUsers, @MaxClients, @MaxTickets)");

        var parameters = new
        {
            plan.Id,
            plan.CreatedAt,
            plan.UpdatedAt,
            plan.InactivedAt,
            plan.Name,
            plan.Description,
            plan.MaxUsers,
            plan.MaxClients,
            plan.MaxTickets
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Plan plan)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Plan.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , name = @Name");
        builder.AppendLine("  , description = @Description");
        builder.AppendLine("  , max_users = @MaxUsers");
        builder.AppendLine("  , max_clients = @MaxClients");
        builder.AppendLine("  , max_tickets = @MaxTickets");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            plan.Id,
            plan.UpdatedAt,
            plan.RowVersion,
            plan.Name,
            plan.Description,
            plan.MaxUsers,
            plan.MaxClients,
            plan.MaxTickets
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Plan plan)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Plan.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            plan.Id,
            plan.RowVersion,
            plan.InactivedAt,
            plan.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
