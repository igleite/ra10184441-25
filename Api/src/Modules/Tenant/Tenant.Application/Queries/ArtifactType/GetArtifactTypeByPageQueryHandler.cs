using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.ArtifactType;

public class GetArtifactTypeByPageQueryHandler : IQueryHandler<GetArtifactTypeByPageQuery, PageDto<ArtifactTypeDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetArtifactTypeByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<ArtifactTypeDto>> Handle(GetArtifactTypeByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(ArtifactTypeDto.Id)}");
        builder.Select($"created_at AS {nameof(ArtifactTypeDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(ArtifactTypeDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(ArtifactTypeDto.RowVersion)}");
        builder.Select($"inactived_at AS {nameof(ArtifactTypeDto.InactivedAt)}");
        builder.Select($"organization_id AS {nameof(ArtifactTypeDto.OrganizationId)}");
        builder.Select($"name AS {nameof(ArtifactTypeDto.Name)}");

        builder.Where("1 = 1");
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName}
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName}
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<ArtifactTypeDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhum tipo de artefato encontrado!");

        return new PageDto<ArtifactTypeDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}
