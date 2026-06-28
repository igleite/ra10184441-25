using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using System.Text;

namespace Identity.Infrastructure.Repositories;

public class VerificationTokenRepository : IVerificationTokenRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public VerificationTokenRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<VerificationToken?> GetByTokenAsync(string token)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(VerificationToken.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(VerificationToken.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(VerificationToken.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(VerificationToken.RowVersion)}");
        builder.AppendLine($"     , token AS {nameof(VerificationToken.Token)}");
        builder.AppendLine($"     , email AS {nameof(VerificationToken.Email)}");
        builder.AppendLine($"     , expires_at AS {nameof(VerificationToken.ExpiresAt)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.VerificationToken.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND token = @Token");

        var parameters = new { Token = token };

        return await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<VerificationToken>(
            builder.ToString(),
            parameters,
            _unitOfWork.SdpDpNew.Transaction);
    }

    public async Task<bool> CreateAsync(VerificationToken verificationToken)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.VerificationToken.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, token, email, expires_at)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @Token, @Email, @ExpiresAt)");

        var parameters = new
        {
            verificationToken.Id,
            verificationToken.CreatedAt,
            verificationToken.UpdatedAt,
            verificationToken.Token,
            verificationToken.Email,
            verificationToken.ExpiresAt
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

        builder.AppendLine($"DELETE FROM {DatabaseSchemaEnum.SdpDpNew.VerificationToken.FullName}");
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
