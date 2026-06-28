using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;

namespace Tickets.Infrastructure.Repositories;

public class StatusReasonRepository : IStatusReasonRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public StatusReasonRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StatusReason?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(StatusReason.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(StatusReason.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(StatusReason.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(StatusReason.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(StatusReason.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(StatusReason.OrganizationId)}");
        builder.AppendLine($"     , type AS {nameof(StatusReason.Type)}");
        builder.AppendLine($"     , name AS {nameof(StatusReason.Name)}");
        builder.AppendLine($"     , is_opening_default AS {nameof(StatusReason.IsOpeningDefault)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.StatusReason.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");

        var parameters = new
        {
            Id = id,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<StatusReason>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<StatusReason?> GetByNameAsync(Guid organizationId, string name)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(StatusReason.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(StatusReason.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(StatusReason.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(StatusReason.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(StatusReason.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(StatusReason.OrganizationId)}");
        builder.AppendLine($"     , type AS {nameof(StatusReason.Type)}");
        builder.AppendLine($"     , name AS {nameof(StatusReason.Name)}");
        builder.AppendLine($"     , is_opening_default AS {nameof(StatusReason.IsOpeningDefault)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.StatusReason.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND name = @Name");
        builder.AppendLine("  AND organization_id = @OrganizationId");

        var parameters = new
        {
            Name = name,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<StatusReason>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<StatusReason?> GetByOpeningDefaultAsync(Guid organizationId)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(StatusReason.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(StatusReason.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(StatusReason.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(StatusReason.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(StatusReason.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(StatusReason.OrganizationId)}");
        builder.AppendLine($"     , type AS {nameof(StatusReason.Type)}");
        builder.AppendLine($"     , name AS {nameof(StatusReason.Name)}");
        builder.AppendLine($"     , is_opening_default AS {nameof(StatusReason.IsOpeningDefault)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.StatusReason.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND is_opening_default = @IsOpeningDefault");

        var parameters = new
        {
            OrganizationId = organizationId,
            IsOpeningDefault = true
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<StatusReason>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(StatusReason statusReason)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.StatusReason.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, organization_id, type, name, is_opening_default)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @OrganizationId, @Type, @Name, @IsOpeningDefault)");

        var parameters = new
        {
            statusReason.Id,
            statusReason.CreatedAt,
            statusReason.UpdatedAt,
            statusReason.InactivedAt,
            statusReason.OrganizationId,
            statusReason.Type,
            statusReason.Name,
            statusReason.IsOpeningDefault
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(StatusReason statusReason)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.StatusReason.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , type = @Type");
        builder.AppendLine("  , name = @Name");
        builder.AppendLine("  , is_opening_default = @IsOpeningDefault");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            statusReason.Id,
            statusReason.UpdatedAt,
            statusReason.OrganizationId,
            statusReason.RowVersion,
            statusReason.Type,
            statusReason.Name,
            statusReason.IsOpeningDefault
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(StatusReason statusReason)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.StatusReason.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            statusReason.Id,
            statusReason.OrganizationId,
            statusReason.RowVersion,
            statusReason.InactivedAt,
            statusReason.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
