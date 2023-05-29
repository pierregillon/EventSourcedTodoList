namespace TimeOnion.Configuration.Authentication;

public record JwtTokenOptions
{
    public const string SectionName = "JWT";

    public string ValidAudience { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public TimeSpan Validity { get; set; } = TimeSpan.FromDays(1);
}