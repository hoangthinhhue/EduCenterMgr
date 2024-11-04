using System.Configuration;
using CleanArchitecture.Blazor.Application;
using CleanArchitecture.Blazor.Infrastructure;
using CleanArchitecture.Blazor.Infrastructure.Constants.Localization;
using Microsoft.AspNetCore.Builder;
using Server.API.Configs;

// program.cs
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAPIService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();