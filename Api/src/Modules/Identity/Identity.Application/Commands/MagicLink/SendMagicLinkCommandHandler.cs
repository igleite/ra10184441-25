using System.Security.Cryptography;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Helpers;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using BuildingBlocks.Application.Interfaces.Services;
using BuildingBlocks.Application.Results;
using Identity.Application.Interfaces;
using Identity.Application.Options;
using Microsoft.Extensions.Options;

namespace Identity.Application.Commands.MagicLink;

public class SendMagicLinkCommandHandler : ICommandHandler<SendMagicLinkCommand>
{
    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IEmailService _emailService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AuthSettings _authSettings;

    public SendMagicLinkCommandHandler(
        IVerificationTokenRepository verificationTokenRepository,
        IEmailService emailService,
        IDateTimeProvider dateTimeProvider,
        IOptions<AuthSettings> authSettings)
    {
        _verificationTokenRepository = verificationTokenRepository;
        _emailService = emailService;
        _dateTimeProvider = dateTimeProvider;
        _authSettings = authSettings.Value;
    }

    public async Task Handle(SendMagicLinkCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim();
        var token = Base64Url.Encode(Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)));
        var expiresAt = _dateTimeProvider.UtcNow.AddMinutes(_authSettings.MagicLinkExpirationMinutes);
        var now = _dateTimeProvider.UtcNow;

        var verificationToken = new Domain.Entities.VerificationToken(Guid.NewGuid(), now);
        verificationToken.SetToken(token, now);
        verificationToken.SetEmail(normalizedEmail, now);
        verificationToken.SetExpiresAt(expiresAt, now);

        var success = await _verificationTokenRepository.CreateAsync(verificationToken);
        if (!success)
            throw AppException.UnprocessableEntity("Não foi possível enviar o link por e-mail.");

        var appBaseUrl = ResolveAppBaseUrl(request.AppOrigin);
        var magicLinkUrl = $"{appBaseUrl}/auth/verify?token={token}";
        var body = $"""
            <h1>Faça login na sua conta</h1>
            <p>Clique no link abaixo para fazer login:</p>
            <a href="{magicLinkUrl}">Faça login agora</a>
            <p>Este link expirará em {_authSettings.MagicLinkExpirationMinutes} minutos.</p>
            """;

        await _emailService.SendAsync([normalizedEmail], "Seu Magic Link", body, cancellationToken: cancellationToken);
    }

    private string ResolveAppBaseUrl(string? appOrigin)
    {
        if (string.IsNullOrWhiteSpace(appOrigin))
            return _authSettings.AppUrl.TrimEnd('/');

        if (!Uri.TryCreate(appOrigin.Trim(), UriKind.Absolute, out var uri))
            throw AppException.BadRequest("Origem inválida.");

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            throw AppException.BadRequest("Origem inválida.");

        if (!IsAllowedAppOrigin(uri))
            throw AppException.BadRequest("Origem inválida.");

        return $"{uri.Scheme}://{uri.Authority}".TrimEnd('/');
    }

    private bool IsAllowedAppOrigin(Uri uri)
    {
        var hostname = uri.Host.ToLowerInvariant();

        if (AppHostHelper.IsReservedInfrastructureHost(hostname))
            return false;

        if (hostname is "localhost" or "127.0.0.1" or "acme.test")
            return true;

        if (hostname.EndsWith(".localhost", StringComparison.Ordinal))
            return true;

        if (hostname is "localtest.me" || hostname.EndsWith(".localtest.me", StringComparison.Ordinal))
            return true;

        if (Uri.TryCreate(_authSettings.AppUrl, UriKind.Absolute, out var configuredAppUrl))
        {
            var configuredHost = configuredAppUrl.Host.ToLowerInvariant();
            if (hostname == configuredHost || hostname.EndsWith($".{configuredHost}", StringComparison.Ordinal))
                return true;
        }

        return false;
    }
}
