using MediatR;
using Mgr.Core.Entities;
using Mgr.Core.Events;
using Mgr.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Uni.Core.Helper;

namespace Uni.Core.Commands
{
    public class DeleteManyBaseCommand<TkeyKey> : IRequest<MethodResult<int>> where TkeyKey : struct
    {
        public List<TkeyKey> Ids { get; set; }
    }

    public class DeleteManyBaseCommandHandler<TDbContext, T, TkeyKey>
       : BaseCommand<TDbContext, T>, IRequestHandler<DeleteManyBaseCommand<TkeyKey>, MethodResult<int>>
         where TDbContext : DbContext
         where T : BaseEntity<TkeyKey>
         where TkeyKey : struct
    {
        private readonly IBaseRepository<TDbContext, T> _repository;

        public DeleteManyBaseCommandHandler()
        {
            _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDbContext, T>>();
        }

        public async Task<MethodResult<int>> Handle(DeleteManyBaseCommand<TkeyKey> request, CancellationToken cancellationToken)
        {
            var deletetedList= await _repository.AllNoTracking.Where(q => request.Ids.Contains(q.Id)).ToListAsync();
            if (deletetedList == null)
                return MethodResult<int>.ErrorNotFoundData();
            foreach (var item in deletetedList)
            {
                item.AddDomainEvent(new DeletedEvent<T>(item));
            }
            await _repository.DeleteRangeAsync(deletetedList);
            var saved = await _UnitOfWork.SaveChangesAsync();
            if (saved > 0)
                return MethodResult<int>.ResultWithData(saved);
            else
                return MethodResult<int>.ErrorBussiness();
        }
    }
}
