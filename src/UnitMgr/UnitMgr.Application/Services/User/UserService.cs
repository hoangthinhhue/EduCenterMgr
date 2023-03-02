using Mgr.Core.Abstracts;
using Mgr.Core.Constants;
using Mgr.Core.Entities;
using Mgr.Core.Entities.Identity;
using Mgr.Core.Exceptions;
using Mgr.Core.Extensions;
using Mgr.Core.Interfaces;
using Mgr.Core.Interfaces.Services;
using Mgr.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnitMgr.Domain.AggregatesModel.IdentityDTOs;
using UnitMgr.Domain.Constants;
using UnitMgr.Infrastructure.Data;

namespace UnitMgr.Application.Services;

public class UserService : BaseService<UnitMgrDbContext,ApplicationUser,Guid>, IUserService
{
    private readonly UserManager<ApplicationUser> _UserManager;
    public UserService(UserManager<ApplicationUser> userManager) : base() {
        _UserManager = userManager;
}
    public async Task<IMethodResult<List<UserDto>>> GetDataCustom(PaginationRequest request)
    {
        var query = _repository.AllNoTracking
                                .Include(q=>q.UserRoles)
                                .ThenInclude(r=>r.Role)
                                .Where(q=>   string.IsNullOrWhiteSpace(request.SearchValue)
                                          || q.UserName!.Contains(request.SearchValue)
                                          || q.DisplayName!.Contains(request.SearchValue)
                                          || q.PhoneNumber!.Contains(request.SearchValue)
                                          || q.Email!.Contains(request.SearchValue)
                                          )
                                .Select(q => new UserDto
                                                        { 
                                                            Id  = q.Id,
                                                            Code = q.Code,
                                                            UserName = q.UserName!,
                                                            PhoneNumber = q.PhoneNumber,
                                                            Email =q.Email!,
                                                            DisplayName =q.DisplayName,
                                                            IsActive= q.IsActive,
                                                            IsLive= q.IsLive,
                                                            AssignedRoles = q.UserRoles.Select(r => r.Role.Name).ToArray(),
                                                        });
        return await query.ToMethodReuslt(request);
    }
    public async Task<IMethodResult> CreateAccount(UserDto model)
    {
        var user =  _mapper.Map<ApplicationUser>(model);
        var pass = UserNameConstants.DefaultPassword;
        if (!string.IsNullOrWhiteSpace(model.PhoneNumber) && model.PhoneNumber.Length > 6)
            pass = model.PhoneNumber;
        user.IsActive= true;
        user.EmailConfirmed= true;
        var rs =await _UserManager.CreateAsync(user, pass);
        if (rs.Succeeded)
        {
            if (model.AssignedRoles != null && model.AssignedRoles.Any())
                await _UserManager.AddToRolesAsync(user, model.AssignedRoles);
        }
        return new MethodResult { Status = (int)HttpStatusCode.OK, Success = rs.Succeeded, Message = string.Join(Environment.NewLine,rs.Errors) };
    }
    public async Task<IMethodResult> UpdateAccount(UserDto model)
    {
        var current = await _repository.FindAsync(q=>q.Id== model.Id) ?? throw new NotFoundException($"Application user not found.");
        var user = _mapper.Map<UserDto,ApplicationUser>(model, current);
        var rs = await _UserManager.UpdateAsync(user);
        if (rs.Succeeded)
        {
            if (model.AssignedRoles != null && model.AssignedRoles.Any())
            {
                var roles = await _UserManager.GetRolesAsync(user);
                await _UserManager.RemoveFromRolesAsync(user, roles);
                await _UserManager.AddToRolesAsync(user, model.AssignedRoles);
            }
        }
        return new MethodResult { Status = (int)HttpStatusCode.OK, Success = rs.Succeeded, Message = string.Join(Environment.NewLine, rs.Errors) };
    }
}
