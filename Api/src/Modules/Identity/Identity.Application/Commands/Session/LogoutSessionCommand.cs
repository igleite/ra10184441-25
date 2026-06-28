using BuildingBlocks.Application.Interfaces.Command;

namespace Identity.Application.Commands.Session;

public record LogoutSessionCommand(string BearerToken) : ICommand;
