using BuildingBlocks.Application.Interfaces.Command;

namespace Identity.Application.Commands.VerificationToken;

public record ConsumeVerificationTokenCommand(string Token) : ICommand<string>;
