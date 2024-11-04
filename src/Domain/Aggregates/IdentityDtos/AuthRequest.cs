namespace CleanArchitecture.Blazor.Domain.Aggregates.IdentityDtos;

public class AuthRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}