using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Organization;

public class GetOrganizationByPageQueryHandler : IQueryHandler<GetOrganizationByPageQuery, PageDto<OrganizationDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<OrganizationDto>> Handle(GetOrganizationByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(OrganizationDto.Id)}");
        builder.Select($"created_at AS {nameof(OrganizationDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(OrganizationDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(OrganizationDto.RowVersion)}");
        builder.Select($"name AS {nameof(OrganizationDto.Name)}");
        builder.Select($"document AS {nameof(OrganizationDto.Document)}");
        builder.Select($"slug AS {nameof(OrganizationDto.Slug)}");

        builder.Where("1 = 1");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.Organization.FullName}
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Organization.FullName}
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<OrganizationDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhuma organização encontrada!");

        return new PageDto<OrganizationDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}
