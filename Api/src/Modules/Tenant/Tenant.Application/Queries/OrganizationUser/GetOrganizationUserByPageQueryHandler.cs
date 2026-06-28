using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.OrganizationUser;

public class GetOrganizationUserByPageQueryHandler : IQueryHandler<GetOrganizationUserByPageQuery, PageDto<OrganizationUserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationUserByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<OrganizationUserDto>> Handle(GetOrganizationUserByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(OrganizationUserDto.Id)}");
        builder.Select($"created_at AS {nameof(OrganizationUserDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(OrganizationUserDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(OrganizationUserDto.RowVersion)}");
        builder.Select($"organization_id AS {nameof(OrganizationUserDto.OrganizationId)}");
        builder.Select($"inactived_at AS {nameof(OrganizationUserDto.InactivedAt)}");
        builder.Select($"user_id AS {nameof(OrganizationUserDto.UserId)}");
        builder.Select($"team_id AS {nameof(OrganizationUserDto.TeamId)}");

        builder.Where("1 = 1");
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.OrganizationUser.FullName}
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.OrganizationUser.FullName}
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<OrganizationUserDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhuma relação entre organização e usuário encontrada!");

        return new PageDto<OrganizationUserDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}

