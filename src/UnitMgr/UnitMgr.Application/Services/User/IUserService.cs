using Mgr.Core.Abstracts;
using Mgr.Core.Entities;
using Mgr.Core.Entities.Identity;
using Mgr.Core.Interfaces;
using Mgr.Core.Interfaces.Services;
using Mgr.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitMgr.Domain.AggregatesModel.IdentityDTOs;
using UnitMgr.Infrastructure.Data;

namespace UnitMgr.Application.Services;

public interface IUserService : IBaseService<UnitMgrDbContext, ApplicationUser, Guid>
{
    Task<IMethodResult<List<UserDto>>> GetDataCustom(PaginationRequest pagingParams);
    Task<IMethodResult> CreateAccount(UserDto model);
    Task<IMethodResult> UpdateAccount(UserDto model);
}
