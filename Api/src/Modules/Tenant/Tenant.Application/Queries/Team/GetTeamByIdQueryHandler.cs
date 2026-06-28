using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Team;

public class GetTeamByIdQueryHandler : IQueryHandler<GetTeamByIdQuery, TeamDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTeamByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TeamDto> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(TeamDto.Id)}");
        builder.Select($"created_at AS {nameof(TeamDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(TeamDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(TeamDto.RowVersion)}");
        builder.Select($"inactived_at AS {nameof(TeamDto.InactivedAt)}");
        builder.Select($"organization_id AS {nameof(TeamDto.OrganizationId)}");
        builder.Select($"name AS {nameof(TeamDto.Name)}");
        builder.Select($"code AS {nameof(TeamDto.Code)}");
        builder.Select($"role_id AS {nameof(TeamDto.RoleId)}");

        builder.Where("1 = 1");
        builder.Where("id = @Id", new { request.Id });
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Team.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<TeamDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"A equipe não existe!");

        return result;
    }
}
