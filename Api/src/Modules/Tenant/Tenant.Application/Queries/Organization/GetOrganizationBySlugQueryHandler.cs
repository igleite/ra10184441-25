using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces.Query;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Organization;

public class GetOrganizationBySlugQueryHandler : IQueryHandler<GetOrganizationBySlugQuery, OrganizationDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationBySlugQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrganizationDto> Handle(GetOrganizationBySlugQuery request, CancellationToken cancellationToken)
    {
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        builder.Select($"id AS {nameof(OrganizationDto.Id)}");
        builder.Select($"created_at AS {nameof(OrganizationDto.CreatedAt)}");
        builder.Select($"updated_at AS {nameof(OrganizationDto.UpdatedAt)}");
        builder.Select($"row_version AS {nameof(OrganizationDto.RowVersion)}");
        builder.Select($"name AS {nameof(OrganizationDto.Name)}");
        builder.Select($"document AS {nameof(OrganizationDto.Document)}");
        builder.Select($"slug AS {nameof(OrganizationDto.Slug)}");

        builder.Where("1 = 1");
        builder.Where("slug = @Slug", new { request.slug });

        builder.OrderBy("created_at DESC");

        var dataSelector = builder.AddTemplate($@"
SELECT 
/**select**/
FROM {DatabaseSchemaEnum.SdpDpNew.Organization.FullName}
/**where**/
/**orderby**/");

        parameters.AddDynamicParams(dataSelector.Parameters);

        var sql = dataSelector.RawSql;

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<OrganizationDto>(sql, parameters, _unitOfWork.SdpDpNew.Transaction, commandTimeout: 30);
        if (result is null)
            throw AppException.NotFound($"A organização não existe!");

        return result;
    }
}
