using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.OrganizationUser;

public class GetOrganizationUserByIdQueryHandler : IQueryHandler<GetOrganizationUserByIdQuery, OrganizationUserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrganizationUserDto> Handle(GetOrganizationUserByIdQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(OrganizationUserDto.Id)}");
        builder.Select($"created_at AS {nameof(OrganizationUserDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(OrganizationUserDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(OrganizationUserDto.RowVersion)}");
        builder.Select($"organization_id AS {nameof(OrganizationUserDto.OrganizationId)}");
        builder.Select($"inactived_at AS {nameof(OrganizationUserDto.InactivedAt)}");
        builder.Select($"user_id AS {nameof(OrganizationUserDto.UserId)}");
        builder.Select($"team_id AS {nameof(OrganizationUserDto.TeamId)}");

        builder.Where("1 = 1");
        builder.Where("id = @Id", new { request.Id });
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.OrganizationUser.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<OrganizationUserDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"A relação entre organização e usuário não existe!");

        return result;
    }
}

