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

public class DeleteForeverBaseCommand<Tkey> : IRequest<MethodResult<int>> where Tkey : struct
{
    public Tkey Id { get; set; }
}

public class DeleteForeverBaseCommandHandler<TDbContext, T, Tkey>
   : BaseCommand<TDbContext, T, Tkey>, IRequestHandler<DeleteForeverBaseCommand<Tkey>, MethodResult<int>>
     where TDbContext : DbContext
     where T : IBaseEntity<Tkey>
     where Tkey : struct
{
    private readonly IBaseRepository<TDbContext, T> _repository;

    public DeleteForeverBaseCommandHandler()
    {
        _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDbContext, T>>();
    }

    public async Task<MethodResult<int>> Handle(DeleteForeverBaseCommand<Tkey> request, CancellationToken cancellationToken)
    {
        var deleteModel = await _repository.FindAsync(q => q.Id.Equals(request.Id));
        deleteModel.AddDomainEvent(new DeletedEvent<T>(deleteModel));
        if (deleteModel == null)
            return MethodResult<int>.ErrorNotFoundData();
        await _repository.DeleteForeverAsync(deleteModel);
        var saved = await _UnitOfWork.SaveChangesAsync();
        if (saved > 0)
            return MethodResult<int>.ResultWithData(saved);
        else
            return MethodResult<int>.ErrorBussiness();
    }
}
