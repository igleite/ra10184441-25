using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.Tickets;

public class GetTicketByIdQueryHandler : IQueryHandler<GetTicketByIdQuery, TicketDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketDto> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(TicketDto.Id)}");
        builder.Select($"created_at AS {nameof(TicketDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(TicketDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(TicketDto.RowVersion)}");
        builder.Select($"deleted_at AS {nameof(TicketDto.DeletedAt)}");
        builder.Select($"organization_id AS {nameof(TicketDto.OrganizationId)}");

        builder.Select($"customer_id AS {nameof(TicketDto.CustomerId)}");
        builder.Select($"artifact_id AS {nameof(TicketDto.ArtifactId)}");
        builder.Select($"status_id AS {nameof(TicketDto.StatusId)}");
        builder.Select($"classification_id AS {nameof(TicketDto.ClassificationId)}");
        builder.Select($"allocation_center AS {nameof(TicketDto.AllocationCenter)}");
        builder.Select($"created_by_user_id AS {nameof(TicketDto.CreatedByUserId)}");
        builder.Select($"description AS {nameof(TicketDto.Description)}");
        builder.Select($"resolution AS {nameof(TicketDto.Resolution)}");

        builder.Where("1 = 1");
        builder.Where("id = @Id", new { request.Id });
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("deleted_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Ticket.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<TicketDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"O ticket não existe!");

        return result;
    }
}
