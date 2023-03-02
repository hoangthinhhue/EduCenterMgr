// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using LazyCache;
using Mgr.Core.Entities;
using Mgr.Core.Entities.Identity;
using Mgr.Core.Exceptions;
using Mgr.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UnitMgr.Domain.AggregatesModel.IdentityDTOs;
using UnitMgr.Domain.Configs;

namespace UnitMgr.Application.Services.Identity;

public class IdentityService : IIdentityService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IOptions<AppConfigurationSettings> _appConfig;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAppCache _cache;
    private readonly IStringLocalizer<IdentityService> _localizer;
    private TimeSpan refreshInterval => TimeSpan.FromSeconds(60);
    private LazyCacheEntryOptions _options => new LazyCacheEntryOptions().SetAbsoluteExpiration(refreshInterval, ExpirationMode.LazyExpiration);
    public IdentityService(
        IServiceScopeFactory scopeFactory,
        IOptions<AppConfigurationSettings> appConfig,
        IAppCache cache,
        IStringLocalizer<IdentityService> localizer)
    {
        _scopeFactory = scopeFactory;
        var scope = _scopeFactory.CreateScope();
        _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        _userClaimsPrincipalFactory = scope.ServiceProvider.GetRequiredService<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _authorizationService = scope.ServiceProvider.GetRequiredService<IAuthorizationService>();
        _appConfig = appConfig;
        _cache = cache;
        _localizer = localizer;
    }

    public async Task<string?> GetUserNameAsync(string userId, CancellationToken cancellation = default)
    {
           var key = $"GetUserNameAsync:{userId}";
            var user = await _cache.GetOrAddAsync(key,async() => await _userManager.Users.SingleOrDefaultAsync(u => u.Id.ToString()  == userId, cancellation),_options);
            return user?.UserName;
    }
    public string GetUserName(string userId)
    {
        var key = $"GetUserName-byId:{userId}";
        var user = _cache.GetOrAdd(key, () => _userManager.Users.SingleOrDefault(u => u.Id.ToString()  == userId), _options);
        return user?.UserName??string.Empty;
    }
    public async Task<bool> IsInRoleAsync(string userId, string role, CancellationToken cancellation = default)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id.ToString()  == userId, cancellation)?? throw new NotFoundException(_localizer["User Not Found."]);
        var result =  await _userManager.IsInRoleAsync(user, role);
        return result;
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName, CancellationToken cancellation = default)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id.ToString()  == userId, cancellation) ?? throw new NotFoundException(_localizer["User Not Found."]);
        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
        var result = await _authorizationService.AuthorizeAsync(principal, policyName);
        return result.Succeeded;

    }

    public async Task<IMethodResult> DeleteUserAsync(string userId, CancellationToken cancellation = default)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id.ToString() == userId, cancellation) ?? throw new NotFoundException(_localizer["User Not Found."]);
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
            return MethodResult.ResultWithSuccess();
        return MethodResult.ResultWithErrors(result.Errors.Select(q=>q.Description).ToArray());
    }
    public async Task<IDictionary<string, string?>> FetchUsers(string roleName, CancellationToken cancellation = default)
    {
        var result = await _userManager.Users
             .Where(x => x.UserRoles.Where(y => y.Role.Name == roleName).Any())
             .Include(x => x.UserRoles)
             .ToDictionaryAsync(x => x.UserName!, y => y.DisplayName, cancellation);
        return result;
    }

    public async Task<IMethodResult<TokenResponse>> LoginAsync(TokenRequest request, CancellationToken cancellation = default)
    {
        var user = await _userManager.FindByNameAsync(request.UserName!);
        if (user == null)
        {
            return  MethodResult<TokenResponse>.ResultWithErrors(new string[] { _localizer["User Not Found."] });
        }
        if (!user.IsActive)
        {
            return  MethodResult<TokenResponse>.ResultWithErrors(new string[] { _localizer["User Not Active. Please contact the administrator."] });
        }
        if (!user.EmailConfirmed)
        {
            return  MethodResult<TokenResponse>.ResultWithErrors(new string[] { _localizer["E-Mail not confirmed."] });
        }
        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password!);
        if (!passwordValid)
        {
            return  MethodResult<TokenResponse>.ResultWithErrors(new string[] { _localizer["Invalid Password."] });
        }
        user.RefreshToken = GenerateRefreshToken();
        var TokenExpiryTime = DateTime.Now.AddDays(7);

        if (request.RememberMe)
        {
            TokenExpiryTime = DateTime.Now.AddYears(1);
        }
        user.RefreshTokenExpiryTime = TokenExpiryTime;
        await _userManager.UpdateAsync(user);
        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
        var token = await GenerateJwtAsync(user, principal.Claims);
        var response = new TokenResponse { Token = token, RefreshTokenExpiryTime = TokenExpiryTime, RefreshToken = user.RefreshToken, ProfilePictureDataUrl = user.ProfilePictureDataUrl };
        return MethodResult<TokenResponse>.ResultWithData(response);
    }

    public async Task<IMethodResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellation = default)
    {
        if (request is null)
        {
            return  MethodResult<TokenResponse>.ResultWithErrors(new string[] { _localizer["Invalid Client Token."] });
        }
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(userEmail!);
        if (user == null)
            return  MethodResult<TokenResponse>.ResultWithErrors(new string[] { _localizer["User Not Found."] });
        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return  MethodResult<TokenResponse>.ResultWithError(_localizer["Invalid Client Token."]);
        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
        var token = GenerateEncryptedToken(GetSigningCredentials(), principal.Claims);
        user.RefreshToken = GenerateRefreshToken();
        await _userManager.UpdateAsync(user);

        var response = new TokenResponse { Token = token, RefreshToken = user.RefreshToken, RefreshTokenExpiryTime = user.RefreshTokenExpiryTime };
        return MethodResult<TokenResponse>.ResultWithData(response);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    private Task<string> GenerateJwtAsync(ApplicationUser user, IEnumerable<Claim> claims)
    {
        var token = GenerateEncryptedToken(GetSigningCredentials(), claims);
        return Task.FromResult(token);
    }
    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddDays(2),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);
        return encryptedToken;
    }
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Value.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException(_localizer["Invalid token"]);
        }
        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(_appConfig.Value.Secret);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    public async Task UpdateLiveStatus(string userId, bool isLive, CancellationToken cancellation = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId) && x.IsLive != isLive);
        if (user is not null)
        {
            user.IsLive = isLive;
            await _userManager.UpdateAsync(user);
        }
    }
    public async Task<UserDto> GetUserDto(string userId, CancellationToken cancellation = default)
    {
        var key = $"GetUserDto:{userId}";
        var x = await _cache.GetOrAddAsync(key, async () => await _userManager.Users.Where(x => x.Id == Guid.Parse(userId)).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(cancellation), _options);
        var userDto = new UserDto()
        {
            Checked = false,
            ProfilePictureDataUrl = x.ProfilePictureDataUrl,
            DisplayName = x.DisplayName,
            Email = x.Email!,
            IsActive = x.IsActive,
            IsLive = x.IsLive,
            PhoneNumber = x.PhoneNumber,
            Provider = x.Provider,
            Id = x.Id,
            UserName = x.UserName!,
            TenantId = x.TenantId,
            TenantName = x.TenantName,
            LockoutEnd = x.LockoutEnd,
            Role = x.UserRoles.Select(x => x.Role.Name).FirstOrDefault(),
            AssignedRoles = x.UserRoles.Select(x => x.Role.Name!).ToArray(),
        };
        return userDto!;
    }
    public async Task<List<UserDto>?> GetUsers(string tenantId, CancellationToken cancellation = default)
    {
        var key = $"GetUsers-tenantId:{tenantId}";
        var result = await _cache.GetOrAddAsync(key, async () => await _userManager.Users.Where(x => x.TenantId.ToString() == tenantId).Include(x => x.UserRoles).ThenInclude(x => x.Role)
           .Select(x => new UserDto()
           {
            Checked = false,
            ProfilePictureDataUrl = x.ProfilePictureDataUrl,
            DisplayName = x.DisplayName,
            Email = x.Email!,
            IsActive = x.IsActive,
            IsLive = x.IsLive,
            PhoneNumber = x.PhoneNumber,
            Provider = x.Provider,
            Id = x.Id,
            UserName = x.UserName!,
            TenantId = x.TenantId,
            TenantName = x.TenantName,
            LockoutEnd = x.LockoutEnd,
            Role = x.UserRoles.Select(x => x.Role.Name).FirstOrDefault(),
            AssignedRoles = x.UserRoles.Select(x => x.Role.Name!).ToArray(),
           }).ToListAsync(), _options);
        
        return result;
    }
}
