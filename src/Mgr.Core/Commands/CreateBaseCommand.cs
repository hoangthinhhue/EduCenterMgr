using MediatR;
using Mgr.Core.Entities;
using Mgr.Core.Events;
using Mgr.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Uni.Core.Helper;

namespace Uni.Core.Commands
{
    public class CreateBaseCommand<T, TkeyKey> : IRequest<MethodResult<TkeyKey>>
       where T : BaseEntity<TkeyKey>
       where TkeyKey : struct
    {
        public T Entity { get; set; }
    }

    public class CreateBaseCommandHandler<TDataContext, T, TKey>
       : BaseCommand<TDataContext, T>, IRequestHandler<CreateBaseCommand<T, TKey>, MethodResult<TKey>>
       where TDataContext : DbContext
       where T : BaseEntity<TKey>
       where TKey : struct
    {
        private readonly IBaseRepository<TDataContext, T> _repository;
        public CreateBaseCommandHandler()
        {
            _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDataContext, T>>();
        }

        public async Task<MethodResult<TKey>> Handle(CreateBaseCommand<T, TKey> request, CancellationToken cancellationToken)
        {
            request.Entity.AddDomainEvent(new CreatedEvent<T>(request.Entity));
            await _repository.InsertAsync(request.Entity);
            var saved = await _UnitOfWork.SaveChangesAsync();
            if (saved > 0)
                return MethodResult<TKey>.ResultWithData(request.Entity.Id);
            else
                return MethodResult<TKey>.ResultWithError(status: (int)System.Net.HttpStatusCode.NotModified);
        }
    }
}
