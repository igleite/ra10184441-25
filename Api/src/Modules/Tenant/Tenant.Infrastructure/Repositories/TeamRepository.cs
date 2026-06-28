using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public TeamRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Team?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Team.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Team.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Team.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Team.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(Team.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(Team.OrganizationId)}");
        builder.AppendLine($"     , name AS {nameof(Team.Name)}");
        builder.AppendLine($"     , code AS {nameof(Team.Code)}");
        builder.AppendLine($"     , role_id AS {nameof(Team.RoleId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Team.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Id = id,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Team>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<Team?> GetByCodeAsync(Guid organizationId, string code)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Team.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Team.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Team.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Team.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(Team.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(Team.OrganizationId)}");
        builder.AppendLine($"     , name AS {nameof(Team.Name)}");
        builder.AppendLine($"     , code AS {nameof(Team.Code)}");
        builder.AppendLine($"     , role_id AS {nameof(Team.RoleId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Team.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND code = @Code");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Code = code,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Team>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<Team?> GetByNameAsync(Guid organizationId, string name)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Team.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Team.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Team.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Team.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(Team.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(Team.OrganizationId)}");
        builder.AppendLine($"     , name AS {nameof(Team.Name)}");
        builder.AppendLine($"     , code AS {nameof(Team.Code)}");
        builder.AppendLine($"     , role_id AS {nameof(Team.RoleId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Team.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND name = @Name");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Name = name,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Team>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(Team team)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.Team.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, organization_id, name, code, role_id)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @OrganizationId, @Name, @Code, @RoleId)");

        var parameters = new
        {
            team.Id,
            team.CreatedAt,
            team.UpdatedAt,
            team.InactivedAt,
            team.OrganizationId,
            team.Name,
            team.Code,
            team.RoleId
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Team team)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Team.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , name = @Name");
        builder.AppendLine("  , code = @Code");
        builder.AppendLine("  , role_id = @RoleId");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            team.Id,
            team.UpdatedAt,
            team.OrganizationId,
            team.RowVersion,
            team.Name,
            team.Code,
            team.RoleId
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Team team)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Team.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            team.Id,
            team.OrganizationId,
            team.RowVersion,
            team.InactivedAt,
            team.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<int> CountActiveByOrganizationIdAsync(Guid organizationId)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT COUNT(1)");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Team.FullName}");
        builder.AppendLine("WHERE organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new { OrganizationId = organizationId };

        return await _unitOfWork.SdpDpNew.Connection.ExecuteScalarAsync<int>(
            builder.ToString(),
            parameters,
            _unitOfWork.SdpDpNew.Transaction);
    }
}
