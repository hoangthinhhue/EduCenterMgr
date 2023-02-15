﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.



using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.Loggers.Caching;
using Mgr.Core.Models;
using static CleanArchitecture.Blazor.Application.Constants.Permissions;

namespace CleanArchitecture.Blazor.Application.Features.Loggers.Commands.Delete;

public class ClearLogsCommand : ICacheInvalidatorRequest<MethodResult>
{

    public string CacheKey => LogsCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => LogsCacheKey.SharedExpiryTokenSource();

}

public class ClearLogsCommandHandler :
             IRequestHandler<ClearLogsCommand, MethodResult>

{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ClearLogsCommandHandler> _localizer;
    private readonly ILogger<ClearLogsCommandHandler> _logger;

    public ClearLogsCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<ClearLogsCommandHandler> localizer,
        ILogger<ClearLogsCommandHandler> logger,
         IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<MethodResult> Handle(ClearLogsCommand request, CancellationToken cancellationToken)
    {
        _context.Loggers.RemoveRange(_context.Loggers);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Logs was erased");
        return await MethodResult.SuccessAsync();
    }

}

