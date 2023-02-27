﻿using Mgr.Core.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using System.Configuration;
using Uni.Core.Helper;
using UnitMgr.Domain.Configs;
using UnitMgr.Infrastructure.Configs.Middlewares;
using UnitMgr.Infrastructure.Hubs;

namespace UnitMgr.Admin.Extensions;

public static class ApplicationConfigure
{
    public static void UseApplicationConfigure(this WebApplication app, IConfiguration configuration)
    {
        //app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseExceptionHandler("/Error");
        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"Files")))
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"Files"));
        }
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
            RequestPath = new PathString("/Files")
        });

        var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(LocalizationConstants.SupportedLanguages.Select(x => x.Code).First())
                  .AddSupportedCultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray())
                  .AddSupportedUICultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());

        HttpContextInfo.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

        app.UseRequestLocalization(localizationOptions);
        //app.UseMiddlewares();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapDefaultControllerRoute();
            endpoints.MapHub<SignalRHub>(SignalR.HubUrl);
        });
    }
    public static void UseMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<LocalizationCookiesMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
    public static void UseConfigureSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", typeof(Program).Assembly.GetName().Name);
            options.RoutePrefix = "swagger";
            options.DisplayRequestDuration();
        });
    }
}
