// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.KeyValues.Caching;
using CleanArchitecture.Blazor.Application.Features.KeyValues.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.KeyValues.Commands.AddEdit;

public class AddEditKeyValueCommand : IMapFrom<KeyValueDto>, ICacheInvalidatorRequest<MethodResult<int>>
{
    public int Id { get; set; }
    public Picklist Name { get; set; }
    public string? Value { get; set; }
    public string? Text { get; set; }
    public string? Description { get; set; }
    public TrackingState TrackingState { get; set; } = TrackingState.Unchanged;
    public string CacheKey => KeyValueCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => KeyValueCacheKey.SharedExpiryTokenSource();
}

public class AddEditKeyValueCommandHandler : IRequestHandler<AddEditKeyValueCommand, MethodResult<int>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddEditKeyValueCommandHandler(
        ApplicationDbContext context,
         IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<MethodResult<int>> Handle(AddEditKeyValueCommand request, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<KeyValueDto>(request);

        if (request.Id > 0)
        {
            var keyValue = await _context.KeyValues.FindAsync(new object[] { request.Id }, cancellationToken);
            _ = keyValue ?? throw new NotFoundException($"KeyValue Pair  {request.Id} Not Found.");
            keyValue = _mapper.Map(dto, keyValue);
            keyValue.AddDomainEvent(new UpdatedEvent<KeyValue>(keyValue));
            await _context.SaveChangesAsync(cancellationToken);
            return MethodResult<int>.Success(keyValue.Id);
        }
        else
        {
            var keyValue = _mapper.Map<KeyValue>(dto);
            keyValue.AddDomainEvent(new UpdatedEvent<KeyValue>(keyValue));
            _context.KeyValues.Add(keyValue);
            await _context.SaveChangesAsync(cancellationToken);
            return await MethodResult<int>.SuccessAsync(keyValue.Id);
        }


    }
}
