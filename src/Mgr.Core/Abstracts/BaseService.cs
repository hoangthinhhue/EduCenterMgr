// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Azure.Core;
using EFCore.BulkExtensions;
using MediatR;
using Mgr.Core.Entities;
using Mgr.Core.Helpers;
using Mgr.Core.Interface;
using Mgr.Core.Interfaces;
using Mgr.Core.Interfaces.Data;
using Mgr.Core.Interfaces.Services;
using Mgr.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Uni.Core.Commands;
using Uni.Core.Helper;

namespace Mgr.Core.Abstracts;
#nullable disable
public abstract class BaseService<TDataContext, T, Tkey> : IBaseService<TDataContext, T, Tkey>
        where TDataContext : DbContext
        where T : BaseEntity<Tkey>
        where Tkey : struct
{
    public readonly IMediator _mediator;
    public readonly IBaseRepository<TDataContext, T> _repository;
    public readonly IUnitOfWork<TDataContext> _uniOfWork;
    public readonly IMapper _mapper;
    public readonly ILogger<IBaseService<TDataContext, T, Tkey>> _appLogger;
    public readonly ICurrentUserService _currentUserService;
    protected BaseService()
    {
        _currentUserService = HttpContextInfo.GetRequestService<ICurrentUserService>();
        _mapper = HttpContextInfo.GetRequestService<IMapper>();
        _appLogger = HttpContextInfo.GetRequestService<ILogger<IBaseService<TDataContext, T, Tkey>>>();
        _uniOfWork = HttpContextInfo.GetRequestService<IUnitOfWork<TDataContext>>();
        _mediator = HttpContextInfo.GetRequestService<IMediator>();
        _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDataContext, T>>();
    }

    public async virtual Task<IMethodResult<Tkey>> InsertAsync(T model)
    {
        try
        {
            CreateBaseCommand<T, Tkey> request = new()
            {
                Entity = model
            };
            var handler = new CreateBaseCommandHandler<TDataContext, T, Tkey>();
            var rs = await handler.Handle(request, default);
            return rs;
        }
        catch (Exception ex)
        {
            return MethodResult<Tkey>.ResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }

    public async virtual Task<IMethodResult<Tkey>> UpdateAsync(T model)
    {
        try
        {
            var oldModel = (await GetByIdAsync(model.Id)).Data;
            UpdateBaseCommand<T, Tkey> request = new()
            {
                Entity = model
            };
            var handler = new UpdateBaseCommandHandler<TDataContext, T, Tkey>();
            var rs = await handler.Handle(request, default);
            if (rs.Success)
            {
                _appLogger.LogInformation("{UserName} đã cập nhật {@oldModel} thành {@model}",_currentUserService.UserName, oldModel, model);
            }
            return rs;
        }
        catch (Exception ex)
        {
            return MethodResult<Tkey>.ResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }

    public async virtual Task<IMethodResult<int>> DeleteAsync(Tkey id)
    {
        try
        {
            var oldModel = (await GetByIdAsync(id)).Data;
            DeleteBaseCommand<Tkey> request = new()
            {
                Id = id
            };
            var handler = new DeleteBaseCommandHandler<TDataContext, T, Tkey>();
            var rs = await handler.Handle(request, default);
            if (rs.Success)
            {
                _appLogger.LogInformation("{UserName} đã xóa {@oldModel}", _currentUserService.UserName, oldModel);
            }
            return rs;
        }
        catch (Exception ex)
        {
            return MethodResult<int>.ResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public async virtual Task<IMethodResult<int>> DeleteForeverAsync(Tkey id)
    {
        try
        {
            var oldModel = (await GetByIdAsync(id)).Data;
            DeleteForeverBaseCommand<Tkey> request = new()
            {
                Id = id
            };
            var handler = new DeleteForeverBaseCommandHandler<TDataContext, T, Tkey>();
            var rs = await handler.Handle(request, default);
            if (rs.Success)
            {
                _appLogger.LogInformation("{UserName} đã xóa {@oldModel}",_currentUserService.UserName, oldModel);
            }
            return rs;
        }
        catch (Exception ex)
        {
            return MethodResult<int>.ResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public async virtual Task<IMethodResult<int>> DeleteManyForeverAsync(IList<Tkey> ids)
    {
        try
        {
            var oldModel = await _repository.AllNoTracking.Where(q => ids.Contains(q.Id)).ToListAsync();

            DeleteManyForeverBaseCommand<Tkey> request = new()
            {
                Ids = ids.ToList()
            };
            var handler = new DeleteManyForeverBaseCommandHandler<TDataContext, T, Tkey>();
            var rs = await handler.Handle(request, default);
            if (rs.Success)
            {
                _appLogger.LogInformation("{UserName} đã xóa nhiều {@oldModel}",_currentUserService.UserName, oldModel);
            }
            return rs;
        }
        catch (Exception ex)
        {
            return MethodResult<int>.ResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public virtual async Task<IMethodResult<T>> GetByIdAsync(Tkey Id)
    {
        try
        {
            GetBaseCommand<T, Tkey> request = new()
            {
                Id = Id
            };
            var handler = new GetBaseCommandHandler<TDataContext, T, Tkey>();
            return await handler.Handle(request, default);
        }
        catch (Exception ex)
        {
            return MethodResult<T>.ResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public async Task<IMethodResult<int>> DeleteManyAsync(List<Tkey> ids)
    {
        try
        {
            var oldModel = await _repository.AllNoTracking.Where(q => ids.Contains(q.Id)).ToListAsync();

            DeleteManyBaseCommand<Tkey> request = new()
            {
                Ids = ids
            };
            var handler = new DeleteManyBaseCommandHandler<TDataContext, T, Tkey>();
            var rs = await handler.Handle(request, default);
            if (rs.Success)
            {
                _appLogger.LogInformation("{UserName} đã xóa nhiều {@oldModel}",_currentUserService.UserName, oldModel);
            }
            return rs;
        }
        catch (Exception ex)
        {
            return MethodResult<int>.ResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }

    public virtual async Task<IMethodResult<IList<T>>> GetData(PaginationRequest pagingParams)
    {
        try
        {
            GetFilterPaginationCommand<T, Tkey> request = new()
            {
                PagingParams = pagingParams
            };
            var handler = new GetFilterPaginationCommandHandler<TDataContext, T, Tkey>();
            return await handler.Handle(request, default);
        }
        catch (Exception ex)
        {
            return MethodResult<IList<T>>.ResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
    public IQueryable<T> GetTable<T>(bool asNoTracking = true) where T : class
    {
        if (asNoTracking)
            return _uniOfWork.CreateDbContext().Set<T>().AsNoTracking();
        return _uniOfWork.CreateDbContext().Set<T>().AsTracking();
    }

    public virtual async Task<IMethodResult> BulkInsertAsync(IList<T> entities, BulkConfig config = null)
    {
        try
        {
            await _repository.BulkInsertAsync(entities, config);
            return MethodResult.ResultWithSuccess();
        }
        catch (Exception ex)
        {
            return MethodResult.ResultWithError(ex.Message);
        }
    }

    public virtual async Task<IMethodResult> BulkInsertOrUpdateAsync(IList<T> entities, BulkConfig config = null)
    {
        try
        {
            await _repository.BulkInsertOrUpdateAsync(entities, config);
            return MethodResult.ResultWithSuccess();
        }
        catch (Exception ex)
        {
            return MethodResult.ResultWithError(ex.Message, (int)HttpStatusCode.BadRequest);
        }
    }

    public virtual async Task<IMethodResult> BulkInsertOrUpdateOrDeleteAsync(IList<T> entities, BulkConfig config = null)
    {
        try
        {
            await _repository.BulkInsertOrUpdateOrDeleteAsync(entities, config);
            return MethodResult.ResultWithSuccess();
        }
        catch (Exception ex)
        {
            return MethodResult.ResultWithError(ex.Message, (int)HttpStatusCode.BadRequest);
        }
    }

    public virtual async Task<IMethodResult> BulkUpdateAsync(IList<T> entities, BulkConfig config = null)
    {
        try
        {
            await _repository.BulkUpdateAsync(entities, config);
            return MethodResult.ResultWithSuccess();
        }
        catch (Exception ex)
        {
            return MethodResult.ResultWithError(ex.Message, (int)HttpStatusCode.BadRequest);
        }
    }

    public virtual async Task<IMethodResult> BulkDeleteAsync(IList<T> entities, BulkConfig config = null)
    {
        try
        {
            await _repository.BulkDeleteAsync(entities, config);
            _appLogger.LogInformation("{UserName} đã xóa nhiều {@entities}",_currentUserService.UserName, entities);
            return MethodResult.ResultWithSuccess();
        }
        catch (Exception ex)
        {
            return MethodResult.ResultWithError(ex.Message, (int)HttpStatusCode.BadRequest);
        }
    }

    public async Task<IMethodResult<IList<T>>> AllAsync(InputModel request)
    {
        try
        {
            var query =  _repository.AllNoTracking.Where(q => q.IsDeleted != true);
            if (request != null && request.FilterParams.Any())
            {
                query = QueryableHelper<T>.GetQuery(query, request.FilterParams) ?? query;
            }

            if (request != null && request.SortingParams != null && request.SortingParams.Any())
            {
                query = SortingHelper<T>.SortData(query, request.SortingParams) ?? query;
            }
            var rs = await query.ToListAsync();
            return MethodResult<IList<T>>.ResultWithData(rs);
        }
        catch (Exception ex)
        {
            return MethodResult<IList<T>>.ResultWithError((int)HttpStatusCode.BadRequest, ex.Message);
        }
    }
}
