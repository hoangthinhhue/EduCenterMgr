// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ClassTypes.Caching;
using CleanArchitecture.Blazor.Application.Features.ClassTypes.DTOs;
using CleanArchitecture.Blazor.Application.Features.ClassTypes.Queries.Specification;

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.Queries.Pagination;

public class ClassTypesWithPaginationQuery : PaginationFilter, ICacheableRequest<PaginatedData<ClassTypeDto>>
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
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ClassTypesWithPaginationQueryHandler> _localizer;

    public ClassTypesWithPaginationQueryHandler(
        ApplicationDbContext context,
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
        var data = await _context.ClassTypes.Specify(new SearchClassTypeSpecification(request))
             .OrderBy($"{request.OrderBy} {request.SortDirection}")
             .ProjectTo<ClassTypeDto>(_mapper.ConfigurationProvider)
             .PaginatedDataAsync(request.PageNumber, request.PageSize);
        return data;
    }
}