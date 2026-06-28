using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.OrganizationPlan;

public class GetOrganizationPlanByPageQueryHandler : IQueryHandler<GetOrganizationPlanByPageQuery, PageDto<OrganizationPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationPlanByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<OrganizationPlanDto>> Handle(GetOrganizationPlanByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(OrganizationPlanDto.Id)}");
        builder.Select($"created_at AS {nameof(OrganizationPlanDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(OrganizationPlanDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(OrganizationPlanDto.RowVersion)}");
        builder.Select($"inactived_at AS {nameof(OrganizationPlanDto.InactivedAt)}");
        builder.Select($"organization_id AS {nameof(OrganizationPlanDto.OrganizationId)}");
        builder.Select($"plan_id AS {nameof(OrganizationPlanDto.PlanId)}");
        builder.Select($"description AS {nameof(OrganizationPlanDto.Description)}");
        builder.Select($"max_users AS {nameof(OrganizationPlanDto.MaxUsers)}");
        builder.Select($"max_clients AS {nameof(OrganizationPlanDto.MaxClients)}");
        builder.Select($"max_tickets AS {nameof(OrganizationPlanDto.MaxTickets)}");

        builder.Where("1 = 1");
        builder.Where("inactived_at IS NULL");
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.OrganizationPlan.FullName}
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.OrganizationPlan.FullName}
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<OrganizationPlanDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhum plano da organização encontrado!");

        return new PageDto<OrganizationPlanDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}

