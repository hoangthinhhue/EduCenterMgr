using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using Mgr.Core.Abstracts;
using UnitMgr.Infrastructure.Data;
using Mgr.Core.Entities;
using UnitMgr.Application.Services;
using Microsoft.AspNetCore.Authorization;
using UnitMgr.Application.Services.Identity;
using Mgr.Core.Interfaces.Services;
using UnitMgr.Domain.AggregatesModel.IdentityDTOs;

namespace UnitMgr.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : BaseController
{
    private readonly ILogger<IdentityController> _logger;
    private readonly IIdentityService _service;
    public IdentityController(ILogger<IdentityController> logger,
                      IIdentityService service) : base()
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    [AllowAnonymous]
    public async virtual Task<IActionResult> Login(TokenRequest request)
    {
        var rs = await _service.LoginAsync(
            request
        );
        return StatusCode(rs);
    }
    [HttpPost("RefreshToken")]
    [Authorize]
    public async virtual Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var rs = await _service.RefreshTokenAsync(
            request
        );
        return StatusCode(rs);
    }
}
