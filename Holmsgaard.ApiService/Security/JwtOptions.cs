namespace Holmsgaard.ApiService.Security;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = "Holmsgaard.Api";
    public string Audience { get; init; } = "Holmsgaard.Web";
    public int ExpirationMinutes { get; init; } = 60;
}
