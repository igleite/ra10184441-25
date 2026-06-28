using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;

namespace Tickets.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public TicketRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Ticket?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Ticket.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Ticket.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Ticket.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Ticket.RowVersion)}");
        builder.AppendLine($"     , deleted_at AS {nameof(Ticket.DeletedAt)}");
        builder.AppendLine($"     , organization_id AS {nameof(Ticket.OrganizationId)}");
        builder.AppendLine($"     , customer_id AS {nameof(Ticket.CustomerId)}");
        builder.AppendLine($"     , artifact_id AS {nameof(Ticket.ArtifactId)}");
        builder.AppendLine($"     , status_id AS {nameof(Ticket.StatusId)}");
        builder.AppendLine($"     , classification_id AS {nameof(Ticket.ClassificationId)}");
        builder.AppendLine($"     , allocation_center AS {nameof(Ticket.AllocationCenter)}");
        builder.AppendLine($"     , created_by_user_id AS {nameof(Ticket.CreatedByUserId)}");
        builder.AppendLine($"     , description AS {nameof(Ticket.Description)}");
        builder.AppendLine($"     , resolution AS {nameof(Ticket.Resolution)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Ticket.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");

        var parameters = new
        {
            Id = id,
            OrganizationId = organizationId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Ticket>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(Ticket ticket)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.Ticket.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, deleted_at, organization_id, customer_id, artifact_id, status_id, classification_id, allocation_center, created_by_user_id, description, resolution)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @DeletedAt, @OrganizationId, @CustomerId, @ArtifactId, @StatusId, @ClassificationId, @AllocationCenter, @CreatedByUserId, @Description, @Resolution)");

        var parameters = new
        {
            ticket.Id,
            ticket.CreatedAt,
            ticket.UpdatedAt,
            ticket.DeletedAt,
            ticket.OrganizationId,
            ticket.CustomerId,
            ticket.ArtifactId,
            ticket.StatusId,
            ticket.ClassificationId,
            AllocationCenter = ticket.AllocationCenter.Value,
            ticket.CreatedByUserId,
            ticket.Description,
            ticket.Resolution
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Ticket ticket)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Ticket.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , customer_id = @CustomerId");
        builder.AppendLine("  , artifact_id = @ArtifactId");
        builder.AppendLine("  , status_id = @StatusId");
        builder.AppendLine("  , classification_id = @ClassificationId");
        builder.AppendLine("  , allocation_center = @AllocationCenter");
        builder.AppendLine("  , created_by_user_id = @CreatedByUserId");
        builder.AppendLine("  , description = @Description");
        builder.AppendLine("  , resolution = @Resolution");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND deleted_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            ticket.Id,
            ticket.UpdatedAt,
            ticket.OrganizationId,
            ticket.RowVersion,
            ticket.CustomerId,
            ticket.ArtifactId,
            ticket.StatusId,
            ticket.ClassificationId,
            AllocationCenter = ticket.AllocationCenter.Value,
            ticket.CreatedByUserId,
            ticket.Description,
            ticket.Resolution
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Ticket ticket)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Ticket.FullName}");
        builder.AppendLine("SET deleted_at = @DeletedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND organization_id = @OrganizationId");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            ticket.Id,
            ticket.OrganizationId,
            ticket.RowVersion,
            ticket.DeletedAt,
            ticket.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
