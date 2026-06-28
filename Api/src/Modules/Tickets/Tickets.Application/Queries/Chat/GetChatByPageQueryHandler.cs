using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.Chat;

public class GetChatByPageQueryHandler : IQueryHandler<GetChatByPageQuery, PageDto<ChatDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetChatByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<ChatDto>> Handle(GetChatByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"c.id AS {nameof(ChatDto.Id)}");
        builder.Select($"c.created_at AS {nameof(ChatDto.CreatedAt)}");
        builder.Select($"c.updated_at AS {nameof(ChatDto.UpdatedAt)}");
        builder.Select($"c.row_version AS {nameof(ChatDto.RowVersion)}");
        builder.Select($"c.deleted_at AS {nameof(ChatDto.DeletedAt)}");

        builder.Select($"c.ticket_id AS {nameof(ChatDto.TicketId)}");
        builder.Select($"c.user_id AS {nameof(ChatDto.UserId)}");
        builder.Select($"c.message AS {nameof(ChatDto.Message)}");

        builder.InnerJoin($"{DatabaseSchemaEnum.SdpDpNew.Ticket.FullName} t ON t.id = c.ticket_id");

        builder.Where("1 = 1");
        builder.Where("c.deleted_at IS NULL");
        builder.Where("t.deleted_at IS NULL");
        builder.Where("t.organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("c.ticket_id = @TicketId", new { request.TicketId });

        builder.OrderBy("c.created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.Chat.FullName} c
/**innerjoin**/
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Chat.FullName} c
/**innerjoin**/
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<ChatDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhum chat encontrado!");

        return new PageDto<ChatDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}
