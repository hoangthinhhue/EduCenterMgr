using Microsoft.AspNetCore.Components.Server.Circuits;
using UnitMgr.Application.Features;
using UnitMgr.Application.Features.UsersState;
using UnitMgr.Infrastructure.Hubs;

namespace UnitMgr.Admin.Extensions;

public static class SignalRServiceCollectionExtensions
{
    public static void AddSignalRServices(this IServiceCollection services)
        => services.AddSingleton<IUsersStateContainer, UsersStateContainer>()
                   .AddScoped<CircuitHandler, CircuitHandlerService>()
                   .AddScoped<HubClient>()
                   .AddSignalR(e => {
                                        e.MaximumReceiveMessageSize = 102400000;
                                    });
}
