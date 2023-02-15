// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using Mgr.Core.Models;
using Mgr.Core.Entities;
using CleanArchitecture.Blazor.Domain.Interfaces;
using Mgr.Core.Events;
using CleanArchitecture.Blazor.Domain.DTOs.Customers.DTOs;
using CleanArchitecture.Domain.Interfaces.Mappings;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.AddEdit;

public class AddEditCustomerCommand : IMapFrom<CustomerDto>, ICacheInvalidatorRequest<MethodResult<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = String.Empty;
    [Description("Description")]
    public string? Description { get; set; }

    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => CustomerCacheKey.SharedExpiryTokenSource();
}

public class AddEditCustomerCommandHandler : IRequestHandler<AddEditCustomerCommand, MethodResult<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditCustomerCommandHandler> _localizer;
    public AddEditCustomerCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditCustomerCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<MethodResult<int>> Handle(AddEditCustomerCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement AddEditCustomerCommandHandler method 
        var dto = _mapper.Map<CustomerDto>(request);
        if (request.Id > 0)
        {
            var item = await _context.Customers.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"The customer [{request.Id}] was not found.");
            item = _mapper.Map(dto, item);
            // raise a update domain event
            item.AddDomainEvent(new UpdatedEvent<Customer>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return MethodResult<int>.ResultWithData(item.Id);
        }
        else
        {
            var item = _mapper.Map<Customer>(dto);
            // raise a create domain event
            item.AddDomainEvent(new CreatedEvent<Customer>(item));
            _context.Customers.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return MethodResult<int>.ResultWithData(item.Id);
        }

    }
}

