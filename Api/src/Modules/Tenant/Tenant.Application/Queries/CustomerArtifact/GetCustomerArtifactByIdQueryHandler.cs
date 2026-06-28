using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.CustomerArtifact;

public class GetCustomerArtifactByIdQueryHandler : IQueryHandler<GetCustomerArtifactByIdQuery, CustomerArtifactDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerArtifactByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerArtifactDto> Handle(GetCustomerArtifactByIdQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"ca.id AS {nameof(CustomerArtifactDto.Id)}");
        builder.Select($"ca.created_at AS {nameof(CustomerArtifactDto.CreatedAt)}");
        builder.Select($"ca.updated_at AS {nameof(CustomerArtifactDto.UpdatedAt)}");
        builder.Select($"ca.row_version AS {nameof(CustomerArtifactDto.RowVersion)}");
        builder.Select($"ca.inactived_at AS {nameof(CustomerArtifactDto.InactivedAt)}");
        builder.Select($"ca.customer_id AS {nameof(CustomerArtifactDto.CustomerId)}");
        builder.Select($"ca.artifact_id AS {nameof(CustomerArtifactDto.ArtifactId)}");

        builder.InnerJoin($"{DatabaseSchemaEnum.SdpDpNew.Customer.FullName} c ON c.id = ca.customer_id");
        builder.InnerJoin($"{DatabaseSchemaEnum.SdpDpNew.Artifact.FullName} a ON a.id = ca.artifact_id");
        builder.InnerJoin($"{DatabaseSchemaEnum.SdpDpNew.ArtifactType.FullName} at ON at.id = a.artifact_type_id");

        builder.Where("1 = 1");
        builder.Where("ca.id = @Id", new { request.Id });
        builder.Where("c.organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("at.organization_id = @OrganizationId");
        builder.Where("ca.inactived_at IS NULL");
        builder.Where("c.inactived_at IS NULL");
        builder.Where("a.inactived_at IS NULL");
        builder.Where("at.inactived_at IS NULL");

        builder.OrderBy("ca.created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.CustomerArtifact.FullName} ca
/**innerjoin**/
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<CustomerArtifactDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"A relação entre cliente e artefato não existe!");

        return result;
    }
}
