using BuildingBlocks.Application.Interfaces.Command;

namespace Identity.Application.Commands.Session;

public record CreateSessionCommand(Guid UserId, string Token, DateTime ExpiresAt) : ICommand;
