// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.



using CleanArchitecture.Blazor.Application.Features.ClassTypes.Caching;

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.Commands.Delete;

public class DeleteClassTypeCommand : ICacheInvalidatorRequest<Result>

{
    public int[] Id { get; }
    public string CacheKey => ClassTypeCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => ClassTypeCacheKey.SharedExpiryTokenSource();
    public DeleteClassTypeCommand(int[] id)
    {
        Id = id;
    }
}


public class DeleteClassTypeCommandHandler :
             IRequestHandler<DeleteClassTypeCommand, Result>
    {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<DeleteClassTypeCommandHandler> _localizer;
    public DeleteClassTypeCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<DeleteClassTypeCommandHandler> localizer,
         IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result> Handle(DeleteClassTypeCommand request, CancellationToken cancellationToken)
    {

        var items = await _context.ClassTypes.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<ClassType>(item));
            _context.ClassTypes.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }
}

