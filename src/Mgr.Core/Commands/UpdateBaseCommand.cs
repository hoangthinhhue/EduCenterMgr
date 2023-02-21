using MediatR;
using Mgr.Core.Events;
using Mgr.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Mgr.Core.Entities;
using Uni.Core.Helper;

namespace Uni.Core.Commands
{
    public class UpdateBaseCommand<T, TkeyKey> : IRequest<MethodResult<TkeyKey>>
         where T : BaseEntity<TkeyKey>
         where TkeyKey : struct
    {
        public T Entity { get; set; }
    }

    public class UpdateBaseCommandHandler<TDataContext, T, TkeyKey>
       : BaseCommand<TDataContext,T>, IRequestHandler<UpdateBaseCommand<T, TkeyKey>, MethodResult<TkeyKey>>
         where TDataContext : DbContext
         where T : BaseEntity<TkeyKey>
         where TkeyKey : struct
    {
        private readonly IBaseRepository<TDataContext, T> _repository;
        public UpdateBaseCommandHandler()
        {
            _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDataContext, T>>();
        }

        public async Task<MethodResult<TkeyKey>> Handle(UpdateBaseCommand<T, TkeyKey> request, CancellationToken cancellationToken)
        {
            var updateModel = await _repository.FindAsync(q => q.Id.Equals(request.Entity.Id));
            updateModel.AddDomainEvent(new UpdatedEvent<T>(updateModel));
            if (updateModel == null)
                return MethodResult<TkeyKey>.ErrorNotFoundData();
            CloneEntity(request.Entity, ref updateModel);
            await _repository.UpdateAsync(updateModel);
            var saved = await _UnitOfWork.SaveChangesAsync();
            if (saved > 0)
                return MethodResult<TkeyKey>.ResultWithData(updateModel.Id);
            else
                return MethodResult<TkeyKey>.ResultWithError(status: (int)System.Net.HttpStatusCode.NotModified, "Not modified");
        }

        private static void CloneEntity(T fromEntity, ref T toEntity)
        {
            string IdName = nameof(fromEntity.Id);
            var props = typeof(T).GetProperties().Where(e => e.CanWrite 
                                                             && e.Name != IdName 
                                                             );
            foreach (var prop in props)
            {
                prop.SetValue(toEntity, prop.GetValue(fromEntity));
            }
        }
    }
}
