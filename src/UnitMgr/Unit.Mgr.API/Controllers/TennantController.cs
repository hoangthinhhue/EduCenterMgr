using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using Mgr.Core.Abstracts;
using UnitMgr.Infrastructure.Data;
using Mgr.Core.Entities;
using UnitMgr.Application.Services;

namespace UnitMgr.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TenantController : BaseController<ITenantService, UnitMgrDbContext, Tenant, Guid>
{
    public TenantController(ILogger<BaseController<ITenantService, UnitMgrDbContext, Tenant, Guid>> logger,
                      ITenantService service) : base(logger, service)
    {
    }

}
