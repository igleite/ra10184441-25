using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using FeatureFlags.Application.DTOs;

namespace FeatureFlags.Application.Queries;

public class GetFeatureFlagByPageQueryHandler : IQueryHandler<GetFeatureFlagByPageQuery, PageDto<FeatureFlagDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFeatureFlagByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<FeatureFlagDto>> Handle(GetFeatureFlagByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(FeatureFlagDto.Id)}");
        builder.Select($"created_at AS {nameof(FeatureFlagDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(FeatureFlagDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(FeatureFlagDto.RowVersion)}");

        builder.Select($"name AS {nameof(FeatureFlagDto.Name)}");
        builder.Select($"description AS {nameof(FeatureFlagDto.Description)}");
        builder.Select($"value AS {nameof(FeatureFlagDto.Value)}");

        builder.Where("1 = 1");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.FeatureFlag.FullName}
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.FeatureFlag.FullName}
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<FeatureFlagDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhuma feature flag encontrada!");

        return new PageDto<FeatureFlagDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}
