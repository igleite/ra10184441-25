using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Plan;

public class GetPlanByPageQueryHandler : IQueryHandler<GetPlanByPageQuery, PageDto<PlanDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPlanByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<PlanDto>> Handle(GetPlanByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(PlanDto.Id)}");
        builder.Select($"created_at AS {nameof(PlanDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(PlanDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(PlanDto.RowVersion)}");
        builder.Select($"inactived_at AS {nameof(PlanDto.InactivedAt)}");
        builder.Select($"name AS {nameof(PlanDto.Name)}");
        builder.Select($"description AS {nameof(PlanDto.Description)}");
        builder.Select($"max_users AS {nameof(PlanDto.MaxUsers)}");
        builder.Select($"max_clients AS {nameof(PlanDto.MaxClients)}");
        builder.Select($"max_tickets AS {nameof(PlanDto.MaxTickets)}");

        builder.Where("1 = 1");
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.Plan.FullName}
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Plan.FullName}
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<PlanDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhum plano encontrado!");

        return new PageDto<PlanDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}

