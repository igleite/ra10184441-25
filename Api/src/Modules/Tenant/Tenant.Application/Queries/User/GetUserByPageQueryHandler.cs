using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.User;

public class GetUserByPageQueryHandler : IQueryHandler<GetUserByPageQuery, PageDto<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<UserDto>> Handle(GetUserByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(UserDto.Id)}");
        builder.Select($"created_at AS {nameof(UserDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(UserDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(UserDto.RowVersion)}");
        builder.Select($"inactived_at AS {nameof(UserDto.InactivedAt)}");
        builder.Select($"name AS {nameof(UserDto.Name)}");
        builder.Select($"email AS {nameof(UserDto.Email)}");

        builder.Where("1 = 1");
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.User.FullName}
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.User.FullName}
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<UserDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhum usuário encontrado!");

        return new PageDto<UserDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}

