using Mgr.Core.Abstracts;
using Mgr.Core.Entities;
using Mgr.Core.Entities.Identity;
using Mgr.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitMgr.Infrastructure.Data;

namespace UnitMgr.Application.Services;

public interface IApplicationRoleService : IBaseService<UnitMgrDbContext, ApplicationRole, Guid>
{
}
