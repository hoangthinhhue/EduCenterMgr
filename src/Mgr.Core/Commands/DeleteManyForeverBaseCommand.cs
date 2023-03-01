using MediatR;
using Mgr.Core.Entities;
using Mgr.Core.Events;
using Mgr.Core.Interface;
using Mgr.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Uni.Core.Helper;

namespace Uni.Core.Commands;

public class DeleteManyForeverBaseCommand<Tkey> : IRequest<MethodResult<int>> where Tkey : struct
{
    public List<Tkey> Ids { get; set; }
}

public class DeleteManyForeverBaseCommandHandler<TDbContext, T, Tkey>
   : BaseCommand<TDbContext, T, Tkey>, IRequestHandler<DeleteManyForeverBaseCommand<Tkey>, MethodResult<int>>
     where TDbContext : DbContext
     where T : IBaseEntity<Tkey>
     where Tkey : struct
{
    private readonly IBaseRepository<TDbContext, T> _repository;

    public DeleteManyForeverBaseCommandHandler()
    {
        _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDbContext, T>>();
    }

    public async Task<MethodResult<int>> Handle(DeleteManyForeverBaseCommand<Tkey> request, CancellationToken cancellationToken)
    {
        var deletetedList = await _repository.ToListAsync(q => request.Ids.Contains(q.Id));
        if (deletetedList.Any())
            return MethodResult<int>.ErrorNotFoundData();
        foreach (var item in deletetedList)
        {
            item.AddDomainEvent(new DeletedEvent<T>(item));
        }
        await _repository.DeleteRangeForeverAsync(deletetedList);
        var saved = await _UnitOfWork.SaveChangesAsync();
        if (saved > 0)
            return MethodResult<int>.ResultWithData(saved);
        else
            return MethodResult<int>.ErrorBussiness();
    }
}
