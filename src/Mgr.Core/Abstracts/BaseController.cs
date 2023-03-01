using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mgr.Core.Entities;
using Mgr.Core.Entities.Identity;
using Mgr.Core.Interface;
using Mgr.Core.Interfaces;
using Mgr.Core.Interfaces.Services;
using Mgr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Azure.Core.HttpHeader;

namespace Mgr.Core.Abstracts;
public class BaseController : ControllerBase
{
    protected UserManager<ApplicationUser> UserManager
    {
        get
        {
            return (UserManager<ApplicationUser>)HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>));
        }
    }

    [NonAction]
    public virtual ActionResult StatusCode(IMethodResult response)
    {
        return StatusCode(response.Status.Value, response);
    }
}

[ApiController]
[Authorize]
public abstract class BaseController<TService, TDataContext, TModel, TPrimary> : BaseController
    where TDataContext : DbContext
    where TService : IBaseService<TDataContext, TModel, TPrimary>
    where TModel : IBaseEntity<TPrimary>
    where TPrimary : struct
{
    public readonly ILogger<BaseController<TService, TDataContext, TModel, TPrimary>> _logger;
    public readonly TService _service;
    protected BaseController(ILogger<BaseController<TService, TDataContext, TModel, TPrimary>> logger,
                          TService service)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async virtual Task<IActionResult> GetById(TPrimary id)
    {
        var rs = await _service.GetByIdAsync(
            id
        );
        return StatusCode(rs);
    }

    /// <summary>
    /// GET ALL PAGING
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("All")]
    public async virtual Task<IActionResult> All([FromBody] InputModel paras)
    {
        var rs = await _service.AllAsync(paras);
        return StatusCode(rs);
    }
    /// <summary>
    /// GET ALL PAGING
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAll")]
    public async virtual Task<IActionResult> GetAll(int pageIndex = 1, int pageSize = 20)
    {

        var rs = await _service.GetData(new PaginationRequest { PageIndex =pageIndex, PageSize = pageSize});
        return StatusCode(rs);
    }

    /// <summary>
    /// INSERT
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async virtual Task<IActionResult> Post([FromBody] TModel model)
    {
        var rs = await _service.InsertAsync(model);
        return StatusCode(rs);
    }

    [HttpPut]
    public async virtual Task<IActionResult> Put([FromBody] TModel model)
    {
        var rs = await _service.UpdateAsync(model);
        return StatusCode(rs);
    }

    [HttpDelete("{id}")]
    public async virtual Task<IActionResult> Delete(TPrimary id)
    {
        var rs = await _service.DeleteAsync(id);
        return StatusCode(rs);
    }

    [HttpPost]
    [Route("DeleteMany")]
    public async virtual Task<IActionResult> DeleteMany(List<TPrimary> ids)
    {
        var rs = await _service.DeleteManyAsync(ids);
        return StatusCode(rs);
    }
    [HttpDelete("DeleteForever/{id}")]
    public async virtual Task<IActionResult> DeleteForever(TPrimary id)
    {
        var rs = await _service.DeleteForeverAsync(id);
        return StatusCode(rs);
    }

    [HttpPost]
    [Route("DeleteForeverMany")]
    public async virtual Task<IActionResult> DeleteForeverMany(List<TPrimary> ids)
    {
        var rs = await _service.DeleteManyForeverAsync(ids);
        return StatusCode(rs);
    }
    /// <summary>
    /// Rename GetFilterPaging API để dễ nhớ
    /// </summary>
    /// <param name="pagingParams"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("GetData")]
    public async virtual Task<IActionResult> GetData([FromBody] PaginationRequest pagingParams)
    {
        var rs = await _service.GetData(pagingParams);
        return StatusCode(rs);
    }
    [HttpPost]
    [Route("GetFilterPaging")]
    public async virtual Task<IActionResult> GetFilterPaging([FromBody] PaginationRequest pagingParams)
    {
        var rs = await _service.GetData(pagingParams);
        return StatusCode(rs);
    }
    [HttpPost]
    [Route("BulkInsertAsync")]
    public async virtual Task<IActionResult> BulkInsertAsync([FromBody] List<TModel> entities)
    {
        var rs = await _service.BulkInsertAsync(entities);
        return StatusCode(rs);
    }
    [HttpPut]
    [Route("BulkUpdateAsync")]
    public async virtual Task<IActionResult> BulkUpdateAsync([FromBody] List<TModel> entities)
    {
        var rs = await _service.BulkUpdateAsync(entities);
        return StatusCode(rs);
    }
    [HttpPost]
    [Route("BulkInsertOrUpdateAsync")]
    public async virtual Task<IActionResult> BulkInsertOrUpdateAsync([FromBody] List<TModel> entities)
    {
        var rs = await _service.BulkInsertOrUpdateAsync(entities);
        return StatusCode(rs);
    }
    [HttpPost]
    [Route("BulkDeleteAsync")]
    public async virtual Task<IActionResult> BulkDeleteAsync([FromBody] List<TModel> entities)
    {
        var rs = await _service.BulkDeleteAsync(entities);
        return StatusCode(rs);
    }
}
