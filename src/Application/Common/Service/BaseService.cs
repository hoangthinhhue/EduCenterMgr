// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq.Expressions;
using System.Net;
using CleanArchitecture.Blazor.Application.Common.Helper;
using CleanArchitecture.Blazor.Domain.Common;
using CleanArchitecture.Blazor.Domain.SeedWork;
using EFCore.BulkExtensions;
using Mgr.Core.Models;

namespace CleanArchitecture.Blazor.Application.Common;
#nullable disable
public abstract class BaseService<TDataContext, TModel, TPrimary> : IBaseService<TDataContext, TModel, TPrimary>
        where TDataContext : DbContext
        where TModel : BaseEntity
        where TPrimary : struct
{
    public readonly IMediator _mediator;
    public readonly IBaseRepository<TDataContext, TModel, TPrimary> _repository;
    public readonly IUnitOfWork<TDataContext> _UnitOfWork;
    public readonly IMapper _Mapper;
    public readonly ILogger<IBaseService<TDataContext, TModel, TPrimary>> _AppLogger;
    protected BaseService()
    {
        _Mapper = HttpContextInfo.GetRequestService<IMapper>();
        _AppLogger = HttpContextInfo.GetRequestService<ILogger<IBaseService<TDataContext, TModel, TPrimary>>>();
        _UnitOfWork = HttpContextInfo.GetRequestService<IUnitOfWork<TDataContext>>();
        _mediator = HttpContextInfo.GetRequestService<IMediator>();
        _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDataContext, TModel, TPrimary>>();
    }

    public async virtual Task<IMethodResult<TPrimary>> InsertAsync(TModel model)
    {
        try
        {
            CreateBaseCommand<TModel, TPrimary> request = new()
            {
                Entity = model
            };
            var handler = new CreateBaseCommandHandler<TDataContext, TModel, TPrimary>();
            var rs = await handler.Handle(request, default);
            return rs;
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<TPrimary>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }

    public async virtual Task<IMethodResult<TPrimary>> UpdateAsync(TModel model)
    {
        try
        {
            var oldModel = (await GetByIdAsync(model.Id)).Data;
            UpdateBaseCommand<TModel, TPrimary> request = new()
            {
                Entity = model
            };
            var handler = new UpdateBaseCommandHandler<TDataContext, TModel, TPrimary>();
            var rs = await handler.Handle(request, default);
            if (rs.Success)
            {
                _AppLogger.Info("{UserName} đã cập nhật {@oldModel} thành {@model}", UserInfo.GetFullName(), oldModel, model);
            }
            return rs;
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<TPrimary>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }

    public async virtual Task<IMethodResult<int>> DeleteAsync(TPrimary id)
    {
        try
        {
            var oldModel = (await GetByIdAsync(id)).Data;
            DeleteBaseCommand<TPrimary> request = new()
            {
                Id = id
            };
            var handler = new DeleteBaseCommandHandler<TDataContext, TModel, TPrimary>();
            var rs = await handler.Handle(request, default);
            if (rs.Success)
            {
                _AppLogger.Info("{UserName} đã xóa {@oldModel}", UserInfo.GetFullName(), oldModel);
            }
            return rs;
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<int>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public async virtual Task<IMethodResult<int>> DeleteForeverAsync(TPrimary id)
    {
        try
        {
            var oldModel = (await GetByIdAsync(id)).Data;
            DeleteForeverCommand<TPrimary> request = new()
            {
                Id = id
            };
            var handler = new DeleteForeverCommandHandler<TDataContext, TModel, TPrimary>();
            var rs = await handler.Handle(request, default);
            if (rs.Success)
            {
                _AppLogger.Info("{UserName} đã xóa {@oldModel}", UserInfo.GetFullName(), oldModel);
            }
            return rs;
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<int>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public async virtual Task<IMethodResult<int>> DeleteManyForeverAsync(List<TPrimary> ids)
    {
        try
        {
            var oldModel = await _repository.AllNoTracking.Where(q => ids.Contains(q.Id)).ToListAsync();

            DeleteManyForeverCommand<TPrimary> request = new()
            {
                Ids = ids
            };
            var handler = new DeleteManyForeverCommandHandler<TDataContext, TModel, TPrimary>();
            var rs = await handler.Handle(request, default);
            if (rs.Success)
            {
                _AppLogger.Info("{UserName} đã xóa nhiều {@oldModel}", UserInfo.GetFullName(), oldModel);
            }
            return rs;
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<int>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public virtual async Task<IMethodResult<TModel>> GetByIdAsync(TPrimary Id)
    {
        try
        {
            GetBaseCommand<TModel, TPrimary> request = new()
            {
                Id = Id
            };
            var handler = new GetBaseCommandHandler<TDataContext, TModel, TPrimary>();
            return await handler.Handle(request, default);
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<TModel>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public virtual async Task<IMethodResult<TModel>> GetByCode(string code)
    {
        try
        {
            GetCodeCommand<TModel, TPrimary> request = new()
            {
                Code = code
            };
            var handler = new GetCodeCommandHandler<TDataContext, TModel, TPrimary>();
            return await handler.Handle(request, default);
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<TModel>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public virtual async Task<IMethodResult<List<TModel>>> ListByCode(string[] codes)
    {
        try
        {
            ListCodeCommand<TModel, TPrimary> request = new()
            {
                Codes = codes
            };
            var handler = new ListCodeCommandHandler<TDataContext, TModel, TPrimary>();
            return await handler.Handle(request, default);
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<List<TModel>>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public async Task<IMethodResult<int>> DeleteManyAsync(List<TPrimary> ids)
    {
        try
        {
            var oldModel = await _repository.AllNoTracking.Where(q => ids.Contains(q.Id)).ToListAsync();

            DeleteManyBaseCommand<TPrimary> request = new()
            {
                Ids = ids
            };
            var handler = new DeleteManyBaseCommandHandler<TDataContext, TModel, TPrimary>();
            var rs = await handler.Handle(request, default);
            if (rs.Success)
            {
                _AppLogger.Info("{UserName} đã xóa nhiều {@oldModel}", UserInfo.GetFullName(), oldModel);
            }
            return rs;
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<int>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Tìm kiếm cơ bản searchValue theo code, name.
    /// Phân trang
    /// </summary>
    /// <param name="searchValue"></param>
    /// <param name="pageIndex"></param>
    /// <param name="PageSize"></param>
    /// <returns></returns>
    public virtual async Task<IMethodResult<List<TModel>>> GetAllAsync(string searchValue, int pageIndex, int PageSize)
    {
        try
        {
            GetAllBaseCommand<TModel, TPrimary> request = new()
            {
                SearchValue = searchValue,
                PageIndex = pageIndex,
                PageSize = PageSize
            };
            var handler = new GetAllBaseCommandHandler<TDataContext, TModel, TPrimary>();
            return await handler.Handle(request, default);
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<List<TModel>>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public virtual async Task<IMethodResult<List<TModel>>> AllAsync(InputModel paras)
    {
        try
        {
            AllBaseCommand<TModel, TPrimary> request = new()
            {
                Params = paras
            };
            var handler = new AllBaseCommandHandler<TDataContext, TModel, TPrimary>();
            return await handler.Handle(request, default);
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<List<TModel>>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public virtual async Task<IMethodResult<List<TModel>>> GetFilterPagingAsync(PaginatedInputModel pagingParams)
    {
        try
        {
            GetFilterPagingBaseCommand<TModel, TPrimary> request = new()
            {
                PagingParams = pagingParams
            };
            var handler = new GetFilterPagingBaseCommandHandler<TDataContext, TModel, TPrimary>();
            return await handler.Handle(request, default);
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<List<TModel>>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public virtual async Task<IMethodResult<List<TModel>>> GetData(PaginatedInputModel pagingParams)
    {
        try
        {
            GetFilterPagingBaseCommand<TModel, TPrimary> request = new()
            {
                PagingParams = pagingParams
            };
            var handler = new GetFilterPagingBaseCommandHandler<TDataContext, TModel, TPrimary>();
            return await handler.Handle(request, default);
        }
        catch (Exception ex)
        {
            return Entities.MethodResult<List<TModel>>.MethodResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public IQueryable<T> GetTable<T>(bool asNoTracking = true) where T : class
    {
        if (asNoTracking)
            return _UnitOfWork.CreateDbContext().Set<T>().AsNoTracking();
        return _UnitOfWork.CreateDbContext().Set<T>().AsTracking();
    }

    public virtual async Task<IMethodResult> BulkInsertAsync(List<TModel> entities, BulkConfig config = null)
    {
        try
        {
            await _repository.BulkInsertAsync(entities, config);
            return MethodResult.MethodResultWithSuccess();
        }
        catch (Exception ex)
        {
            return MethodResult.ErrorBussiness(ex.Message);
        }
    }

    public virtual async Task<IMethodResult> BulkInsertOrUpdateAsync(List<TModel> entities, BulkConfig config = null)
    {
        try
        {
            await _repository.BulkInsertOrUpdateAsync(entities, config);
            return MethodResult.MethodResultWithSuccess();
        }
        catch (Exception ex)
        {
            return MethodResult.MethodResultWithError(ex.Message, (int)HttpStatusCode.BadRequest);
        }
    }

    public virtual async Task<IMethodResult> BulkInsertOrUpdateOrDeleteAsync(List<TModel> entities, BulkConfig config = null)
    {
        try
        {
            await _repository.BulkInsertOrUpdateOrDeleteAsync(entities, config);
            return MethodResult.MethodResultWithSuccess();
        }
        catch (Exception ex)
        {
            return MethodResult.MethodResultWithError(ex.Message, (int)HttpStatusCode.BadRequest);
        }
    }

    public virtual async Task<IMethodResult> BulkUpdateAsync(List<TModel> entities, BulkConfig config = null)
    {
        try
        {
            await _repository.BulkUpdateAsync(entities, config);
            return MethodResult.MethodResultWithSuccess();
        }
        catch (Exception ex)
        {
            return MethodResult.MethodResultWithError(ex.Message, (int)HttpStatusCode.BadRequest);
        }
    }

    public virtual async Task<IMethodResult> BulkDeleteAsync(List<TModel> entities, BulkConfig config = null)
    {
        try
        {
            await _repository.BulkDeleteAsync(entities, config);
            _AppLogger.Info("{UserName} đã xóa nhiều {@entities}", UserInfo.GetFullName(), entities);
            return MethodResult.MethodResultWithSuccess();
        }
        catch (Exception ex)
        {
            return MethodResult.MethodResultWithError(ex.Message, (int)HttpStatusCode.BadRequest);
        }
    }
}
