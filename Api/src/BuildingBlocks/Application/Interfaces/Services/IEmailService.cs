namespace BuildingBlocks.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendAsync(IEnumerable<string> to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default);
}
