// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ClassTypes.Caching;
using CleanArchitecture.Blazor.Domain.DTOs.ClassTypes.DTOs;
using CleanArchitecture.Blazor.Domain.Interfaces;
using Mgr.Core.Extensions;
using Mgr.Core.Models;

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.Queries.Pagination;

public class ClassTypesWithPaginationQuery : PaginationRequest, ICacheableRequest<PaginatedData<ClassTypeDto>>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int? Duration { get; set; }
    public override string ToString()
    {
        return $"{base.ToString()},Name:{Name},Code:{Code},Description:{Description}";
    }
    public string CacheKey => ClassTypeCacheKey.GetPaginationCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => ClassTypeCacheKey.MemoryCacheEntryOptions;
}

public class ClassTypesWithPaginationQueryHandler :
         IRequestHandler<ClassTypesWithPaginationQuery, PaginatedData<ClassTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ClassTypesWithPaginationQueryHandler> _localizer;

    public ClassTypesWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<ClassTypesWithPaginationQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<PaginatedData<ClassTypeDto>> Handle(ClassTypesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.ClassTypes
             .ProjectTo<ClassTypeDto>(_mapper.ConfigurationProvider)
             .ToPageListAsync(request);
        return data;
    }
}