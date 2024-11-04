namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public interface IJwtTokenSettings
{
    string ValidIssuer { get; set; }
    string ValidAudience { get; set; }
    string SymmetricSecurityKey { get; set; }
    string JwtRegisteredClaimNamesSub { get; set; }
}