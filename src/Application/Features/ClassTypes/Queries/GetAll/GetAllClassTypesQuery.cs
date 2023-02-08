// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ClassTypes.Caching;
using CleanArchitecture.Blazor.Application.Features.ClassTypes.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.Queries.GetAll;

public class GetAllClassTypesQuery : ICacheableRequest<IEnumerable<ClassTypeDto>>
{
    public string CacheKey => ClassTypeCacheKey.GetAllCacheKey;
    public MemoryCacheEntryOptions? Options => ClassTypeCacheKey.MemoryCacheEntryOptions;
}

public class GetAllClassTypesQueryHandler :
     IRequestHandler<GetAllClassTypesQuery, IEnumerable<ClassTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetAllClassTypesQueryHandler> _localizer;

    public GetAllClassTypesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetAllClassTypesQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<IEnumerable<ClassTypeDto>> Handle(GetAllClassTypesQuery request, CancellationToken cancellationToken)
    {
        //TODO:Implementing GetAllClassTypesQueryHandler method 
        var data = await _context.ClassTypes
                     .ProjectTo<ClassTypeDto>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
        return data;
    }
}


