using Mgr.Core.Entities;
using Mgr.Core.Interfaces;
using Mgr.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitMgr.Domain.AggregatesModel.IdentityDTOs;
using UnitMgr.Infrastructure.Data;

namespace UnitMgr.Application.Services.Identity;

public interface IIdentityService : IBaseService
{
    Task<IMethodResult<TokenResponse>> LoginAsync(TokenRequest request, CancellationToken cancellation = default);
    Task<IMethodResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellation = default);
    Task<string?> GetUserNameAsync(string userId, CancellationToken cancellation = default);
    Task<bool> IsInRoleAsync(string userId, string role, CancellationToken cancellation = default);
    Task<bool> AuthorizeAsync(string userId, string policyName, CancellationToken cancellation = default);
    Task<IMethodResult> DeleteUserAsync(string userId, CancellationToken cancellation = default);
    Task<IDictionary<string, string?>> FetchUsers(string roleName, CancellationToken cancellation = default);
    Task UpdateLiveStatus(string userId, bool isLive, CancellationToken cancellation = default);
    Task<UserDto> GetUserDto(string userId, CancellationToken cancellation = default);
    string GetUserName(string userId);
    Task<List<UserDto>?> GetUsers(string tenantId, CancellationToken cancellation = default);
}
