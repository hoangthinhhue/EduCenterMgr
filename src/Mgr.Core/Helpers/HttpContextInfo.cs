using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Uni.Core.Helper;
public static class HttpContextInfo
{
    private static IHttpContextAccessor? _httpContextAccessor;

    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public static HttpContext Current => _httpContextAccessor.HttpContext;

    public static TService GetRequestService<TService>()
    {
        return (TService)Current.RequestServices.GetService(typeof(TService));
    }
}