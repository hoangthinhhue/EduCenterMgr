using MediatR;
using Mgr.Core.Events;
using Mgr.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Mgr.Core.Entities;
using Uni.Core.Helper;
using Mgr.Core.Interface;

namespace Uni.Core.Commands
{
    public class UpdateBaseCommand<T, Tkey> : IRequest<MethodResult<Tkey>>
         where T : IBaseEntity<Tkey>
         where Tkey : struct
    {
        public T Entity { get; set; }
    }

    public class UpdateBaseCommandHandler<TDataContext, T, Tkey>
       : BaseCommand<TDataContext,T,Tkey>, IRequestHandler<UpdateBaseCommand<T, Tkey>, MethodResult<Tkey>>
         where TDataContext : DbContext
         where T : IBaseEntity<Tkey>
         where Tkey : struct
    {
        private readonly IBaseRepository<TDataContext, T> _repository;
        public UpdateBaseCommandHandler()
        {
            _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDataContext, T>>();
        }

        public async Task<MethodResult<Tkey>> Handle(UpdateBaseCommand<T, Tkey> request, CancellationToken cancellationToken)
        {
            var updateModel = await _repository.FindAsync(q => q.Id.Equals(request.Entity.Id));
            updateModel.AddDomainEvent(new UpdatedEvent<T>(updateModel));
            if (updateModel == null)
                return MethodResult<Tkey>.ErrorNotFoundData();
            CloneEntity(request.Entity, ref updateModel);
            await _repository.UpdateAsync(updateModel);
            var saved = await _UnitOfWork.SaveChangesAsync();
            if (saved > 0)
                return MethodResult<Tkey>.ResultWithData(updateModel.Id);
            else
                return MethodResult<Tkey>.ResultWithError(status: (int)System.Net.HttpStatusCode.NotModified, "Not modified");
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
