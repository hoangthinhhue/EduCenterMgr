// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using BlazorState;
using CleanArchitecture.Blazor.Application.Common.Behaviours;
using CleanArchitecture.Blazor.Application.Common.Interfaces.MultiTenant;
using CleanArchitecture.Blazor.Application.Common.Security;
using MediatR;
using Mgr.Core.Abstracts;
using Mgr.Core.Interface;
using Mgr.Core.Interfaces.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Uni.Core.Commands;
using static CleanArchitecture.Blazor.Application.Features.Identity.Profile.UserProfileState;

namespace CleanArchitecture.Blazor.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());

        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //services.AddBlazorState((options) => options.Assemblies = new Assembly[] {
        //    Assembly.GetExecutingAssembly(),
        //});
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MemoryCacheBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        //add base
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped(typeof(IBaseRepository<,,>), typeof(BaseRepository<,,>));

        services.AddLazyCache();
        services.AddScoped<RegisterFormModelFluentValidator>();
        return services;
    }
   
}
