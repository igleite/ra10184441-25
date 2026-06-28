using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Artifact;

public class GetArtifactByIdQueryHandler : IQueryHandler<GetArtifactByIdQuery, ArtifactDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetArtifactByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ArtifactDto> Handle(GetArtifactByIdQuery request, CancellationToken cancellationToken)
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
        builder.Where("a.id = @Id", new { request.Id });
        builder.Where("at.organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("a.inactived_at IS NULL");
        builder.Where("at.inactived_at IS NULL");

        builder.OrderBy("a.created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Artifact.FullName} a
/**innerjoin**/
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<ArtifactDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"O artefato não existe!");

        return result;
    }
}
