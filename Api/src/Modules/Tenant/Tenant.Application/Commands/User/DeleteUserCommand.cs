using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.User;

public record DeleteUserCommand(Guid Id, byte[] RowVersion) : ICommand<UserDto>;

