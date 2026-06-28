using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public UserRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(User.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(User.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(User.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(User.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(User.InactivedAt)}");
        builder.AppendLine($"     , name AS {nameof(User.Name)}");
        builder.AppendLine($"     , email AS {nameof(User.Email)}");
        builder.AppendLine($"     , role_id AS {nameof(User.RoleId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.User.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Id = id
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<User>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(User.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(User.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(User.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(User.RowVersion)}");
        builder.AppendLine($"     , inactived_at AS {nameof(User.InactivedAt)}");
        builder.AppendLine($"     , name AS {nameof(User.Name)}");
        builder.AppendLine($"     , email AS {nameof(User.Email)}");
        builder.AppendLine($"     , role_id AS {nameof(User.RoleId)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.User.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND email = @Email");
        builder.AppendLine("  AND inactived_at IS NULL");

        var parameters = new
        {
            Email = email
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<User>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(User user)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.User.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, inactived_at, name, email, role_id)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @InactivedAt, @Name, @Email, @RoleId)");

        var parameters = new
        {
            user.Id,
            user.CreatedAt,
            user.UpdatedAt,
            user.InactivedAt,
            user.Name,
            user.Email,
            user.RoleId
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.User.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , name = @Name");
        builder.AppendLine("  , email = @Email");
        builder.AppendLine("  , role_id = @RoleId");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND inactived_at IS NULL");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            user.Id,
            user.UpdatedAt,
            user.RowVersion,
            user.Name,
            user.Email,
            user.RoleId
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(User user)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.User.FullName}");
        builder.AppendLine("SET inactived_at = @InactivedAt");
        builder.AppendLine("  , updated_at = @UpdatedAt");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            user.Id,
            user.RowVersion,
            user.InactivedAt,
            user.UpdatedAt
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
