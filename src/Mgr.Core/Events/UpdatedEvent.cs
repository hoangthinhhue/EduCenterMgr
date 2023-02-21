using Mgr.Core.Abstracts;
using Mgr.Core.Entities;

namespace Mgr.Core.Events;
public class UpdatedEvent<T> : DomainEvent where T : BaseEntity
{
    public UpdatedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}
