// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Documents.Caching;
using CleanArchitecture.Blazor.Application.Common.Interfaces.MultiTenant;
using Mgr.Core.Models;
using Mgr.Core.Interface;
using Mgr.Core.Extensions;
using CleanArchitecture.Blazor.Domain.Interfaces;
using CleanArchitecture.Blazor.Domain.DTOs.Documents.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Documents.Queries.PaginationQuery;

public class DocumentsWithPaginationQuery : PaginationRequest, ICacheableRequest<PaginatedData<DocumentDto>>
{
   
    public string TenantId { get; set; }
    public DocumentsWithPaginationQuery(string tenantId)
    {
        TenantId = tenantId;
    }
    public string CacheKey => $"{nameof(DocumentsWithPaginationQuery)},{this},{TenantId}";
    public MemoryCacheEntryOptions? Options => DocumentCacheKey.MemoryCacheEntryOptions;

}
public class DocumentsQueryHandler : IRequestHandler<DocumentsWithPaginationQuery, PaginatedData<DocumentDto>>
{
    private readonly ICurrentUserService _currentUserService;
 
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DocumentsQueryHandler(
        ICurrentUserService currentUserService,
  
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _currentUserService = currentUserService;
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedData<DocumentDto>> Handle(DocumentsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var userid = _currentUserService.UserId;
        var data = await _context.Documents
            .ProjectTo<DocumentDto>(_mapper.ConfigurationProvider)
            .ToPageListAsync(request);
        return data;
    }
}
