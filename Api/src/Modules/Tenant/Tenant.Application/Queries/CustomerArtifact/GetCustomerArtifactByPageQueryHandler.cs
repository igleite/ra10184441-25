using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.CustomerArtifact;

public class GetCustomerArtifactByPageQueryHandler : IQueryHandler<GetCustomerArtifactByPageQuery, PageDto<CustomerArtifactDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerArtifactByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<CustomerArtifactDto>> Handle(GetCustomerArtifactByPageQuery request, CancellationToken cancellationToken)
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
        builder.Where("c.organization_id = @OrganizationId", new { request.OrganizationId });
        builder.Where("at.organization_id = @OrganizationId");
        builder.Where("ca.inactived_at IS NULL");
        builder.Where("c.inactived_at IS NULL");
        builder.Where("a.inactived_at IS NULL");
        builder.Where("at.inactived_at IS NULL");

        if (request.CustomerId.HasValue)
            builder.Where("ca.customer_id = @CustomerId", new { request.CustomerId });

        builder.OrderBy("ca.created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.CustomerArtifact.FullName} ca
/**innerjoin**/
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.CustomerArtifact.FullName} ca
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
        var data = await multi.ReadAsync<CustomerArtifactDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhuma relação entre cliente e artefato encontrada!");

        return new PageDto<CustomerArtifactDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}
