using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;

namespace Tickets.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Chat?> GetByIdAsync(Guid organizationId, Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       c.id AS {nameof(Chat.Id)}");
        builder.AppendLine($"     , c.created_at AS {nameof(Chat.CreatedAt)}");
        builder.AppendLine($"     , c.updated_at AS {nameof(Chat.UpdatedAt)}");
        builder.AppendLine($"     , c.row_version AS {nameof(Chat.RowVersion)}");
        builder.AppendLine($"     , c.deleted_at AS {nameof(Chat.DeletedAt)}");
        builder.AppendLine($"     , c.ticket_id AS {nameof(Chat.TicketId)}");
        builder.AppendLine($"     , c.user_id AS {nameof(Chat.UserId)}");
        builder.AppendLine($"     , c.message AS {nameof(Chat.Message)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Chat.FullName} c");
        builder.AppendLine($"INNER JOIN {DatabaseSchemaEnum.SdpDpNew.Ticket.FullName} t ON t.id = c.ticket_id");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND t.organization_id = @OrganizationId");
        builder.AppendLine("  AND c.id = @Id");
        builder.AppendLine("  AND c.deleted_at IS NULL");
        builder.AppendLine("  AND t.deleted_at IS NULL");

        var parameters = new
        {
            OrganizationId = organizationId,
            Id = id
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Chat>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<Chat?> GetByTicketIdAsync(Guid ticketId)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Chat.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Chat.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Chat.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Chat.RowVersion)}");
        builder.AppendLine($"     , deleted_at AS {nameof(Chat.DeletedAt)}");
        builder.AppendLine($"     , ticket_id AS {nameof(Chat.TicketId)}");
        builder.AppendLine($"     , user_id AS {nameof(Chat.UserId)}");
        builder.AppendLine($"     , message AS {nameof(Chat.Message)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Chat.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND ticket_id = @TicketId");
        builder.AppendLine("  AND deleted_at IS NULL");

        var parameters = new
        {
            TicketId = ticketId
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Chat>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(Chat chat)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.Chat.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, deleted_at, ticket_id, user_id, message)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @DeletedAt, @TicketId, @UserId, @Message)");

        var parameters = new
        {
            chat.Id,
            chat.CreatedAt,
            chat.UpdatedAt,
            chat.DeletedAt,
            chat.TicketId,
            chat.UserId,
            chat.Message
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Chat chat)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Chat.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , ticket_id = @TicketId");
        builder.AppendLine("  , user_id = @UserId");
        builder.AppendLine("  , message = @Message");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND deleted_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            chat.Id,
            chat.UpdatedAt,
            chat.RowVersion,
            chat.TicketId,
            chat.UserId,
            chat.Message
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Chat chat)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Chat.FullName}");
        builder.AppendLine("SET deleted_at = @DeletedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            chat.Id,
            chat.RowVersion,
            chat.DeletedAt,
            chat.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
