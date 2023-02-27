using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitMgr.Application.Features;

public class InMemoryTicketStore : ITicketStore
{
    private readonly IMemoryCache _cache;

    public InMemoryTicketStore(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(_cache)); ;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);

        return Task.CompletedTask;
    }

    public Task<AuthenticationTicket> RetrieveAsync(string key)
    {
        _cache.TryGetValue(key, out AuthenticationTicket ticket);
        return Task.FromResult(ticket);
    }

    public Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        var options = new MemoryCacheEntryOptions().SetSize(1);
        var expiresUtc = ticket.Properties.ExpiresUtc;
        if (expiresUtc.HasValue)
        {
            options.SetAbsoluteExpiration(expiresUtc.Value);
        }

        if (ticket.Properties.AllowRefresh ?? false)
        {
            options.SetSlidingExpiration(TimeSpan.FromMinutes(60));//TODO: configurable.
        }

        _cache.Set(key, ticket, options);

        return Task.FromResult(0);
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var key = ticket.Principal.Claims
          .First(c => c.Type == ClaimTypes.Name).Value;
        await RenewAsync(key, ticket);
        return key;
    }
}
