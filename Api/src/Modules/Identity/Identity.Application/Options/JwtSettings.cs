namespace Identity.Application.Options;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "Api";
    public string Audience { get; set; } = "Api";
    public int ExpirationMinutes { get; set; } = 60;
}
