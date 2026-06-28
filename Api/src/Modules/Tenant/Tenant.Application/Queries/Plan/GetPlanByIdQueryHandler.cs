using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Plan;

public class GetPlanByIdQueryHandler : IQueryHandler<GetPlanByIdQuery, PlanDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPlanByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PlanDto> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
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
        builder.Where("id = @Id", new { request.Id });
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Plan.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<PlanDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"O plano não existe!");

        return result;
    }
}

