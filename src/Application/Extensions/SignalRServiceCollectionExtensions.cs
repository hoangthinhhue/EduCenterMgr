using CleanArchitecture.Blazor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Blazor.Application.Services.CircuitHandle;
using CleanArchitecture.Blazor.Application.Services.Identity;
using CleanArchitecture.Blazor.Infrastructure.Hubs;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Blazor.Application.Extensions;
public static class SignalRServiceCollectionExtensions
{
    public static void AddSignalRServices(this IServiceCollection services)
        => services.AddSingleton<IUsersStateContainer, UsersStateContainer>()
                   .AddScoped<CircuitHandler, CircuitHandlerService>()
                   .AddScoped<HubClient>()
                   .AddSignalR();
}
