using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.User;

public record GetUserByIdQuery(Guid Id) : IQuery<UserDto>;

