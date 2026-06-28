using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.StatusReason;

public class GetStatusReasonByIdQueryHandler : IQueryHandler<GetStatusReasonByIdQuery, StatusReasonDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStatusReasonByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StatusReasonDto> Handle(GetStatusReasonByIdQuery request, CancellationToken cancellationToken)
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
        builder.Where("id = @Id", new { request.Id });
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.StatusReason.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<StatusReasonDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"O motivo de status não existe!");

        return result;
    }
}

