using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.Chat;

public class GetChatByTicketIdQueryHandler : IQueryHandler<GetChatByTicketIdQuery, ChatDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetChatByTicketIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ChatDto> Handle(GetChatByTicketIdQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(ChatDto.Id)}");
        builder.Select($"created_at AS {nameof(ChatDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(ChatDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(ChatDto.RowVersion)}");
        builder.Select($"deleted_at AS {nameof(ChatDto.DeletedAt)}");

        builder.Select($"ticket_id AS {nameof(ChatDto.TicketId)}");
        builder.Select($"user_id AS {nameof(ChatDto.UserId)}");
        builder.Select($"message AS {nameof(ChatDto.Message)}");

        builder.Where("1 = 1");
        builder.Where("ticket_id = @Id", new { request.TicketId });
        builder.Where("deleted_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Chat.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<ChatDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"O chat não existe!");

        return result;
    }
}
