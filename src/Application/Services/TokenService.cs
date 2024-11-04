using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Blazor.Application.Services;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Blazor.Domain.Identity;
using Microsoft.Extensions.Configuration;
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

public class TokenService : ITokenService
{
    private const int ExpirationMinutes = 60;
    private readonly ILogger<TokenService> _logger;
    private readonly IConfiguration _configuration;

    public TokenService(ILogger<TokenService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public string CreateToken(ApplicationUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        
        _logger.LogInformation("JWT Token created");
        
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private List<Claim> CreateClaims(ApplicationUser user)
    {        
        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            foreach (var role in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var symmetricSecurityKey = _configuration.GetSection("JwtTokenSettings")["SymmetricSecurityKey"];
        
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }
}