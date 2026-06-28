namespace Identity.Application.Options;

public class AuthSettings
{
    public const string SectionName = "Auth";

    public string AppUrl { get; set; } = "http://localhost:3000";
    public int MagicLinkExpirationMinutes { get; set; } = 15;
}
