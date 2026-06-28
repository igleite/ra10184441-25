using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tickets.Application.DTOs;

namespace Tickets.Application.Queries.Chat;

public class GetChatByIdQueryHandler : IQueryHandler<GetChatByIdQuery, ChatDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetChatByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ChatDto> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
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
        builder.Where("c.id = @Id", new { request.Id });
        builder.Where("c.deleted_at IS NULL");
        builder.Where("t.deleted_at IS NULL");
        builder.Where("t.organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("c.ticket_id = @TicketId", new { request.TicketId });

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Chat.FullName} c
/**innerjoin**/
/**where**/
ORDER BY c.created_at DESC");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<ChatDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"O chat não existe!");

        return result;
    }
}
