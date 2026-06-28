using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;

namespace Tickets.Infrastructure.Repositories;

public class TicketClassificationRepository : ITicketClassificationRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public TicketClassificationRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketClassification?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(TicketClassification.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(TicketClassification.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(TicketClassification.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(TicketClassification.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(TicketClassification.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(TicketClassification.OrganizationId)}");
        builder.AppendLine($"     , name AS {nameof(TicketClassification.Name)}");
        builder.AppendLine($"     , code AS {nameof(TicketClassification.Code)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.TicketClassification.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Id = id,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<TicketClassification>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<TicketClassification?> GetByCodeAsync(Guid organizationId, string code)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(TicketClassification.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(TicketClassification.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(TicketClassification.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(TicketClassification.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(TicketClassification.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(TicketClassification.OrganizationId)}");
        builder.AppendLine($"     , name AS {nameof(TicketClassification.Name)}");
        builder.AppendLine($"     , code AS {nameof(TicketClassification.Code)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.TicketClassification.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND code = @Code");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Code = code,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<TicketClassification>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<TicketClassification?> GetByNameAsync(Guid organizationId, string name)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(TicketClassification.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(TicketClassification.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(TicketClassification.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(TicketClassification.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(TicketClassification.InactivedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(TicketClassification.OrganizationId)}");
        builder.AppendLine($"     , name AS {nameof(TicketClassification.Name)}");
        builder.AppendLine($"     , code AS {nameof(TicketClassification.Code)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.TicketClassification.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND name = @Name");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Name = name,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<TicketClassification>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(TicketClassification ticketClassification)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.TicketClassification.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, organization_id, name, code)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @OrganizationId, @Name, @Code)");

        var parameters = new
        {
            ticketClassification.Id,
            ticketClassification.CreatedAt,
            ticketClassification.UpdatedAt,
            ticketClassification.InactivedAt,
            ticketClassification.OrganizationId,
            ticketClassification.Name,
            ticketClassification.Code
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(TicketClassification ticketClassification)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.TicketClassification.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , name = @Name");
        builder.AppendLine("  , code = @Code");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            ticketClassification.Id,
            ticketClassification.UpdatedAt,
            ticketClassification.OrganizationId,
            ticketClassification.RowVersion,
            ticketClassification.Name,
            ticketClassification.Code
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(TicketClassification ticketClassification)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.TicketClassification.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            ticketClassification.Id,
            ticketClassification.OrganizationId,
            ticketClassification.RowVersion,
            ticketClassification.InactivedAt,
            ticketClassification.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
