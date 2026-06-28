using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Role?> GetByIdAsync(Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Role.Id)}");
        builder.AppendLine($"     , name AS {nameof(Role.Name)}");
        builder.AppendLine($"     , scope AS {nameof(Role.Scope)}");
        builder.AppendLine($"     , created_at AS {nameof(Role.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Role.UpdatedAt)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Role.FullName}");
        builder.AppendLine("WHERE id = @Id");

        var parameters = new { Id = id };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Role>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }
}
