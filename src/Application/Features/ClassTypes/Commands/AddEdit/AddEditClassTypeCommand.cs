// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using CleanArchitecture.Blazor.Application.Features.Documents.DTOs;
using CleanArchitecture.Blazor.Application.Features.ClassTypes.Caching;
using CleanArchitecture.Blazor.Application.Features.ClassTypes.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using CleanArchitecture.Core.Models;

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.Commands.AddEdit;

public class AddEditClassTypeCommand : IMapFrom<ClassTypeDto>, ICacheInvalidatorRequest<MethodResult<int>>
{

    public int Id { get; set; }
    /// <summary>
    /// MÃ
    /// </summary>
    public string? Code { get; set; }
    /// <summary>
    /// TÊN
    /// </summary>
    public string? Name { get; set; }
    public string? Description { get; set; }
    /// <summary>
    /// Số giờ giảng dạy dự kiến
    /// </summary>
    public int? Duration { get; set; }
    public string CacheKey => ClassTypeCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => ClassTypeCacheKey.SharedExpiryTokenSource();
}

public class AddEditClassTypeCommandHandler : IRequestHandler<AddEditClassTypeCommand, MethodResult<int>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public AddEditClassTypeCommandHandler(
        ApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<MethodResult<int>> Handle(AddEditClassTypeCommand request, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<ClassTypeDto>(request);
        if (request.Id > 0)
        {
            var item = await _context.ClassTypes.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"ClassType {request.Id} Not Found.");
            item = _mapper.Map(dto, item);
            item.AddDomainEvent(new UpdatedEvent<ClassType>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await MethodResult<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = _mapper.Map<ClassType>(dto);
            item.AddDomainEvent(new CreatedEvent<ClassType>(item));
            _context.ClassTypes.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await MethodResult<int>.SuccessAsync(item.Id);
        }

    }
}

