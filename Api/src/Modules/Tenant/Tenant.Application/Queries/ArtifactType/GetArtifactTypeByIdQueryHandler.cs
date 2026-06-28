using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.ArtifactType;

public class GetArtifactTypeByIdQueryHandler : IQueryHandler<GetArtifactTypeByIdQuery, ArtifactTypeDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetArtifactTypeByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ArtifactTypeDto> Handle(GetArtifactTypeByIdQuery request, CancellationToken cancellationToken)
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
        builder.Where("id = @Id", new { request.Id });
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("inactived_at IS NULL");

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<ArtifactTypeDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"O tipo de artefato não existe!");

        return result;
    }
}
