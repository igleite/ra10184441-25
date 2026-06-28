using BuildingBlocks.Application.Queries;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.User;

public record GetUserByPageQuery(int Page, int PerPage) : PageQuery<UserDto>(Page, PerPage);

