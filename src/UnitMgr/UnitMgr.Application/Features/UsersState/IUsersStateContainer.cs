using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitMgr.Application.Features.UsersState;
public interface IUsersStateContainer
{
    ConcurrentDictionary<string, string> UsersByConnectionId { get; }
    event Action? OnChange;
    void Update(string connectionId, string? name);
    void Remove(string connectionId);
}
