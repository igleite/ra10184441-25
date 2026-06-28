using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.StatusReason;

public class GetStatusReasonByPageQueryHandler : IQueryHandler<GetStatusReasonByPageQuery, PageDto<StatusReasonDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStatusReasonByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<StatusReasonDto>> Handle(GetStatusReasonByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(StatusReasonDto.Id)}");
        builder.Select($"created_at AS {nameof(StatusReasonDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(StatusReasonDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(StatusReasonDto.RowVersion)}");
        builder.Select($"inactived_at AS {nameof(StatusReasonDto.InactivedAt)}");
        builder.Select($"organization_id AS {nameof(StatusReasonDto.OrganizationId)}");
        builder.Select($"type AS {nameof(StatusReasonDto.Type)}");
        builder.Select($"name AS {nameof(StatusReasonDto.Name)}");
        builder.Select($"is_opening_default AS {nameof(StatusReasonDto.IsOpeningDefault)}");

        builder.Where("1 = 1");
        builder.Where("inactived_at IS NULL");
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.StatusReason.FullName}
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.StatusReason.FullName}
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<StatusReasonDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhum motivo de status encontrado!");

        return new PageDto<StatusReasonDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}

