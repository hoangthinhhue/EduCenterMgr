using CleanArchitecture.Blazor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Blazor.Infrastructure.Hubs;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace CleanArchitecture.Blazor.Infrastructure.Extensions;
public static class SignalRServiceCollectionExtensions
{
    public static void AddSignalRServices(this IServiceCollection services)
        => services.AddSingleton<IUsersStateContainer, IUsersStateContainer>()
                   .AddScoped<CircuitHandler, CircuitHandlerService>()
                   .AddScoped<HubClient>()
                   .AddSignalR();
}
