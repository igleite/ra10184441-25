using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.User;

public record UpdateUserCommand(Guid Id, string Name, string Email, RoleBaseEnum Role, byte[] RowVersion) : ICommand<UserDto>;

