using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Customer;

public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
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
        builder.Where("id = @Id", new { request.Id });

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Customer.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<CustomerDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"O cliente não existe!");

        return result;
    }
}
