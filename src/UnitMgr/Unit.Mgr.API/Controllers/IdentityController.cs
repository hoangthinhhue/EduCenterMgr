using Microsoft.AspNetCore.Mvc;
using Mgr.Core.Abstracts;
using Microsoft.AspNetCore.Authorization;
using UnitMgr.Domain.AggregatesModel.IdentityDTOs;
using UnitMgr.Application.Services.Identity;

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
