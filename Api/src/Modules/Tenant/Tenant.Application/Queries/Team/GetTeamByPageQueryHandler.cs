using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Team;

public class GetTeamByPageQueryHandler : IQueryHandler<GetTeamByPageQuery, PageDto<TeamDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTeamByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<TeamDto>> Handle(GetTeamByPageQuery request, CancellationToken cancellationToken)
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
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.Team.FullName}
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Team.FullName}
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<TeamDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhuma equipe encontrada!");

        return new PageDto<TeamDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}
