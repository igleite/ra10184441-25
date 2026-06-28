using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using FeatureFlags.Application.DTOs;

namespace FeatureFlags.Application.Queries;

public class GetFeatureFlagByNameQueryHandler : IQueryHandler<GetFeatureFlagByNameQuery, FeatureFlagDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFeatureFlagByNameQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<FeatureFlagDto> Handle(GetFeatureFlagByNameQuery request, CancellationToken cancellationToken)
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
        builder.Where("name = @Name", new { Name = request.Name });

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.FeatureFlag.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<FeatureFlagDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"A feature flag não existe!");

        return result;
    }
}

