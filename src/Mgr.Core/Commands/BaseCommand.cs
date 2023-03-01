using AutoMapper;
using Mgr.Core.Entities;
using Mgr.Core.Interface;
using Mgr.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using Uni.Core.Helper;

namespace Uni.Core.Commands;

public abstract class BaseCommand<TDataContext,T,Tkey> : IBaseCommand<TDataContext,T,Tkey>
    where TDataContext : DbContext
    where T : IBaseEntity<Tkey>
    where Tkey : struct
{
    protected readonly IMapper _Mapper;
    protected readonly ILogger<IBaseCommand<TDataContext,T,Tkey>> _AppLogger;
    protected readonly IUnitOfWork<TDataContext> _UnitOfWork;
    protected readonly IBaseRepository<TDataContext, T> _Repos;
    protected BaseCommand()
    {
        _Mapper = HttpContextInfo.GetRequestService<IMapper>();
        _AppLogger = HttpContextInfo.GetRequestService<ILogger<IBaseCommand<TDataContext,T,Tkey>>>();
        _UnitOfWork = HttpContextInfo.GetRequestService<IUnitOfWork<TDataContext>>();
        _Repos = HttpContextInfo.GetRequestService<IBaseRepository<TDataContext, T>>();
    }

    public IQueryable<Entity> GetTable<Entity> (bool asNoTracking = true) where Entity : class
    {
        if(asNoTracking)
            return _UnitOfWork.CreateDbContext().Set<Entity>().AsNoTracking();
        return _UnitOfWork.CreateDbContext().Set<Entity>().AsTracking();
    }
}
