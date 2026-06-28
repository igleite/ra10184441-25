using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using System.Text;

namespace Identity.Infrastructure.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public SessionRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Session?> GetByTokenAsync(string token)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Session.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Session.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Session.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Session.RowVersion)}");
        builder.AppendLine($"     , user_id AS {nameof(Session.UserId)}");
        builder.AppendLine($"     , token AS {nameof(Session.Token)}");
        builder.AppendLine($"     , expires_at AS {nameof(Session.ExpiresAt)}");
        builder.AppendLine($"     , last_used_at AS {nameof(Session.LastUsedAt)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Session.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND token = @Token");

        var parameters = new { Token = token };

        return await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Session>(
            builder.ToString(),
            parameters,
            _unitOfWork.SdpDpNew.Transaction);
    }

    public async Task<bool> CreateAsync(Session session)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.Session.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, user_id, token, expires_at, last_used_at)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @UserId, @Token, @ExpiresAt, @LastUsedAt)");

        var parameters = new
        {
            session.Id,
            session.CreatedAt,
            session.UpdatedAt,
            session.UserId,
            session.Token,
            session.ExpiresAt,
            session.LastUsedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(
            builder.ToString(),
            parameters,
            _unitOfWork.SdpDpNew.Transaction);

        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Session session)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Session.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , last_used_at = @LastUsedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            session.Id,
            session.UpdatedAt,
            session.LastUsedAt,
            session.RowVersion
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(
            builder.ToString(),
            parameters,
            _unitOfWork.SdpDpNew.Transaction);

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteByTokenAsync(string token)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"DELETE FROM {DatabaseSchemaEnum.SdpDpNew.Session.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND token = @Token");

        var parameters = new { Token = token };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(
            builder.ToString(),
            parameters,
            _unitOfWork.SdpDpNew.Transaction);

        return rowsAffected > 0;
    }
}
