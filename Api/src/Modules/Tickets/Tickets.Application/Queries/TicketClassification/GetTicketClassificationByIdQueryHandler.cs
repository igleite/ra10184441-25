using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.TicketClassification;

public class GetTicketClassificationByIdQueryHandler : IQueryHandler<GetTicketClassificationByIdQuery, TicketClassificationDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketClassificationByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketClassificationDto> Handle(GetTicketClassificationByIdQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(TicketClassificationDto.Id)}");
        builder.Select($"created_at AS {nameof(TicketClassificationDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(TicketClassificationDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(TicketClassificationDto.RowVersion)}");
        builder.Select($"inactived_at AS {nameof(TicketClassificationDto.InactivedAt)}");
        builder.Select($"organization_id AS {nameof(TicketClassificationDto.OrganizationId)}");
        builder.Select($"name AS {nameof(TicketClassificationDto.Name)}");
        builder.Select($"code AS {nameof(TicketClassificationDto.Code)}");

        builder.Where("1 = 1");
        builder.Where("id = @Id", new { request.Id });
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.TicketClassification.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<TicketClassificationDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"A classificação não existe!");

        return result;
    }
}
