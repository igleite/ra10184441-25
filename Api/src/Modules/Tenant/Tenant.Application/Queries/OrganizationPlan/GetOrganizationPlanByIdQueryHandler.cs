using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.OrganizationPlan;

public class GetOrganizationPlanByIdQueryHandler : IQueryHandler<GetOrganizationPlanByIdQuery, OrganizationPlanDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationPlanByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrganizationPlanDto> Handle(GetOrganizationPlanByIdQuery request, CancellationToken cancellationToken)
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
        builder.Where("id = @Id", new { request.Id });
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.OrganizationPlan.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<OrganizationPlanDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"O plano da organização não existe!");

        return result;
    }
}

