using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Artifact;

public class GetArtifactByPageQueryHandler : IQueryHandler<GetArtifactByPageQuery, PageDto<ArtifactDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetArtifactByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<ArtifactDto>> Handle(GetArtifactByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"a.id AS {nameof(ArtifactDto.Id)}");
        builder.Select($"a.created_at AS {nameof(ArtifactDto.CreatedAt)}");
        builder.Select($"a.updated_at AS {nameof(ArtifactDto.UpdatedAt)}");
        builder.Select($"a.row_version AS {nameof(ArtifactDto.RowVersion)}");
        builder.Select($"a.inactived_at AS {nameof(ArtifactDto.InactivedAt)}");
        builder.Select($"a.artifact_type_id AS {nameof(ArtifactDto.ArtifactTypeId)}");
        builder.Select($"a.name AS {nameof(ArtifactDto.Name)}");
        builder.Select($"a.code AS {nameof(ArtifactDto.Code)}");

        builder.InnerJoin($"{DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName} at ON at.id = a.artifact_type_id");

        builder.Where("1 = 1");
        builder.Where("at.organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("a.inactived_at IS NULL");
        builder.Where("at.inactived_at IS NULL");

        if (request.ArtifactTypeId.HasValue)
            builder.Where("a.artifact_type_id = @ArtifactTypeId", new { request.ArtifactTypeId });

        builder.OrderBy("a.created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName} a
/**innerjoin**/
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName} a
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
        var data = await multi.ReadAsync<ArtifactDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhum artefato encontrado!");

        return new PageDto<ArtifactDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}
