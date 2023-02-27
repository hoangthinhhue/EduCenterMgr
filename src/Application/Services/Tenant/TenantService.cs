using Mgr.Core.Abstracts;
using Mgr.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitMgr.Infrastructure.Data;

namespace UnitMgr.Application.Services
{
    public class TenantService : BaseService<UnitMgrDbContext, Tenant, Guid>,
                                ITenantService
    {
    }
}
