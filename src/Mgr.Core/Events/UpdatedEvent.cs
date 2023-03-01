using Mgr.Core.Abstracts;
using Mgr.Core.Entities;
using Mgr.Core.Interface;

namespace Mgr.Core.Events;
public class UpdatedEvent<T> : DomainEvent where T : IBaseEntity
{
    public UpdatedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}
