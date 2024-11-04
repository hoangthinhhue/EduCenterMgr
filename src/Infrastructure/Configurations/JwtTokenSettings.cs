namespace CleanArchitecture.Blazor.Infrastructure.Configurations;

public class JwtTokenSettings : IJwtTokenSettings
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public string SymmetricSecurityKey { get; set; }
    public string JwtRegisteredClaimNamesSub { get; set; }
}