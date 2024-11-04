namespace CleanArchitecture.Blazor.Domain.Aggregates.IdentityDtos;

public class AuthResponse
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
}