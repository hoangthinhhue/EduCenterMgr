using System.Net;
using Mgr.Core.Entities;
using Mgr.Core.Exceptions;
using Mgr.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;


namespace CleanArchitecture.Blazor.Infrastructure.Middlewares;

internal class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IStringLocalizer<ExceptionHandlingMiddleware> _localizer;

    public ExceptionHandlingMiddleware(
  
        ICurrentUserService currentUserService ,
        ILogger<ExceptionHandlingMiddleware> logger,
        IStringLocalizer<ExceptionHandlingMiddleware> localizer)
    {
        _currentUserService = currentUserService;
        _logger = logger;
        _localizer = localizer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var userId = _currentUserService.UserId;
            var responseModel = MethodResult.ResultWithError(exception.Message);
            var response = context.Response;
            response.ContentType = "application/json";
            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }
            if (!string.IsNullOrEmpty(exception.Message))
            {
                responseModel.Message= exception.Message;
            }
            switch (exception)
            {
                case CustomException e:
                    response.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null)
                    {
                        responseModel.Message = string.Join("; ",e.ErrorMessages);
                    }
                    break;
                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            //_logger.LogError(exception, $"{exception}. Request failed with Status Code {response.StatusCode}");
            await response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(responseModel));
        }
    }
}
