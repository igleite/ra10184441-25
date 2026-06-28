using BuildingBlocks.Application.Enums;
using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.User;

public record CreateUserCommand(string Name, string Email, RoleBaseEnum Role) : ICommand<UserDto>;

