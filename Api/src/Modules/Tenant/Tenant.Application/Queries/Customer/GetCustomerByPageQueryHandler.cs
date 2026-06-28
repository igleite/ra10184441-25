using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Customer;

public class GetCustomerByPageQueryHandler : IQueryHandler<GetCustomerByPageQuery, PageDto<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerByPageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageDto<CustomerDto>> Handle(GetCustomerByPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(CustomerDto.Id)}");
        builder.Select($"created_at AS {nameof(CustomerDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(CustomerDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(CustomerDto.RowVersion)}");
        builder.Select($"inactived_at AS {nameof(CustomerDto.InactivedAt)}");
        builder.Select($"organization_id AS {nameof(CustomerDto.OrganizationId)}");
        builder.Select($"name AS {nameof(CustomerDto.Name)}");
        builder.Select($"document AS {nameof(CustomerDto.Document)}");

        builder.Where("1 = 1");
        builder.Where("inactived_at IS NULL");
        builder.Where("organization_id = @OrganizationId", new { request.OrganizationId });

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT COUNT(*)
FROM {DatabaseSchemaEnum.SdpDpNew.Customer.FullName}
/**where**/;

SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Customer.FullName}
/**where**/
/**orderby**/
{_unitOfWork.SdpDpNew.Offset()}");

        parameters.AddDynamicParams(dataSelector.Parameters);
        parameters.Add("@PageIndex", request.PageIndex - 1);
        parameters.Add("@PageSize", request.PageSize);

        var sql = dataSelector.RawSql;

        using var multi = await _unitOfWork.SdpDpNew.Connection.QueryMultipleAsync(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        var totalItemCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<CustomerDto>();

        if (data is null || !data.Any())
            throw AppException.NotFound($"Nenhum cliente encontrado!");

        return new PageDto<CustomerDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            Items = data
        };
    }
}
