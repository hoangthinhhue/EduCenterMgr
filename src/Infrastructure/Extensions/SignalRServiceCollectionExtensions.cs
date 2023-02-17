using CleanArchitecture.Blazor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Blazor.Domain.Services.Identity;
using CleanArchitecture.Blazor.Infrastructure.Hubs;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace CleanArchitecture.Blazor.Infrastructure.Extensions;
public static class SignalRServiceCollectionExtensions
{
    public static void AddSignalRServices(this IServiceCollection services)
        => services.AddSingleton<IUsersStateContainer, UsersStateContainer>()
                   .AddScoped<CircuitHandler, CircuitHandlerService>()
                   .AddScoped<HubClient>()
                   .AddSignalR();
}
