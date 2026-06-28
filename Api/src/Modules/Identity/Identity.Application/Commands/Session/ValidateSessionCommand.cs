using BuildingBlocks.Application.Interfaces.Command;

namespace Identity.Application.Commands.Session;

public record ValidateSessionCommand(string BearerToken) : ICommand<Guid>;
