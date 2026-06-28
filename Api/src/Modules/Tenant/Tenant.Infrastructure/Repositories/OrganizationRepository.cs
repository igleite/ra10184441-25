using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;
using System.Text;
using Tenant.Application.Interfaces;
using Tenant.Domain.Entities;

namespace Tenant.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public OrganizationRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Organization?> GetByIdAsync(Guid id)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Organization.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Organization.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Organization.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Organization.RowVersion)}");
        builder.AppendLine($"     , name AS {nameof(Organization.Name)}");
        builder.AppendLine($"     , document AS {nameof(Organization.Document)}");
        builder.AppendLine($"     , slug AS {nameof(Organization.Slug)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Organization.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");

        var parameters = new
        {
            Id = id
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Organization>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<Organization?> GetBySlugAsync(string slug)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Organization.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Organization.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Organization.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Organization.RowVersion)}");
        builder.AppendLine($"     , name AS {nameof(Organization.Name)}");
        builder.AppendLine($"     , document AS {nameof(Organization.Document)}");
        builder.AppendLine($"     , slug AS {nameof(Organization.Slug)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Organization.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND slug = @Slug");

        var parameters = new
        {
            Slug = slug
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Organization>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<Organization?> GetByDocumentAsync(string document)
    {
        var builder = new StringBuilder();

        builder.AppendLine("SELECT");
        builder.AppendLine($"       id AS {nameof(Organization.Id)}");
        builder.AppendLine($"     , created_at AS {nameof(Organization.CreatedAt)}");
        builder.AppendLine($"     , updated_at AS {nameof(Organization.UpdatedAt)}");
        builder.AppendLine($"     , row_version AS {nameof(Organization.RowVersion)}");
        builder.AppendLine($"     , name AS {nameof(Organization.Name)}");
        builder.AppendLine($"     , document AS {nameof(Organization.Document)}");
        builder.AppendLine($"     , slug AS {nameof(Organization.Slug)}");
        builder.AppendLine($"FROM {DatabaseSchemaEnum.SdpDpNew.Organization.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND document = @Document");

        var parameters = new
        {
            Document = document
        };

        var result = await _unitOfWork.SdpDpNew.Connection.QueryFirstOrDefaultAsync<Organization>(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);

        return result;
    }

    public async Task<bool> CreateAsync(Organization organization)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"INSERT INTO {DatabaseSchemaEnum.SdpDpNew.Organization.FullName}");
        builder.AppendLine("       (id, created_at, updated_at, name, document, slug)");
        builder.AppendLine("VALUES");
        builder.AppendLine("       (@Id, @CreatedAt, @UpdatedAt, @Name, @Document, @Slug)");

        var parameters = new
        {
            organization.Id,
            organization.CreatedAt,
            organization.UpdatedAt,
            organization.Name,
            organization.Document,
            organization.Slug
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Organization organization)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"UPDATE {DatabaseSchemaEnum.SdpDpNew.Organization.FullName}");
        builder.AppendLine("SET updated_at = @UpdatedAt");
        builder.AppendLine("  , name = @Name");
        builder.AppendLine("  , document = @Document");
        builder.AppendLine("  , slug = @Slug");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            organization.Id,
            organization.UpdatedAt,
            organization.RowVersion,
            organization.Name,
            organization.Document,
            organization.Slug
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Organization organization)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"DELETE FROM {DatabaseSchemaEnum.SdpDpNew.Organization.FullName}");
        builder.AppendLine("WHERE 1 = 1");
        builder.AppendLine("  AND id = @Id");
        builder.AppendLine("  AND row_version = @RowVersion");

        var parameters = new
        {
            organization.Id,
            organization.RowVersion,
        };

        var rowsAffected = await _unitOfWork.SdpDpNew.Connection.ExecuteAsync(builder.ToString(), parameters, _unitOfWork.SdpDpNew.Transaction);
        return rowsAffected > 0;
    }
}
