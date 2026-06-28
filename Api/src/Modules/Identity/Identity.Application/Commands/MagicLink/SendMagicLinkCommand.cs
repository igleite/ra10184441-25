using BuildingBlocks.Application.Interfaces.Command;

namespace Identity.Application.Commands.MagicLink;

public record SendMagicLinkCommand(string Email, string? AppOrigin = null) : ICommand;
