// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Documents.Caching;
using Mgr.Core.Models;
using Mgr.Core.Entities;
using Mgr.Core.EnumType;
using Uni.Core.Commands;
using Azure.Core;
using Mgr.Core.Events;
using CleanArchitecture.Blazor.Infrastructure.Persistence;
using CleanArchitecture.Blazor.Domain.Interfaces;
using CleanArchitecture.Blazor.Domain.DTOs.Documents.DTOs;
using CleanArchitecture.Domain.Interfaces.Mappings;

namespace CleanArchitecture.Blazor.Application.Features.Documents.Commands.AddEdit;

public class AddEditDocumentCommand :IMapFrom<DocumentDto>, ICacheInvalidatorRequest<MethodResult<int>>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public string? URL { get; set; }
    public DocumentType DocumentType { get; set; } = DocumentType.Document;
    public string? TenantId { get; set; }
    public string? TenantName { get; set; }
    public CancellationTokenSource? SharedExpiryTokenSource => DocumentCacheKey.SharedExpiryTokenSource();
    public UploadRequest? UploadRequest { get; set; }
  
}

public class AddEditDocumentCommandHandler : BaseCommand<ApplicationDbContext,Document,int>,IRequestHandler<AddEditDocumentCommand, MethodResult<int>>
{
    private readonly IUploadService _uploadService;

    public AddEditDocumentCommandHandler(
         IMapper mapper,
         IUploadService uploadService
        ) : base()
    {
    }
    public async Task<MethodResult<int>> Handle(AddEditDocumentCommand request, CancellationToken cancellationToken)
    {
        var dto = _Mapper.Map<DocumentDto>(request);
        if (request.Id > 0)
        {
            var document = await _Repos.FindAsync(q=>q.Id == request.Id);
            _ = document ?? throw new NotFoundException($"Document {request.Id} Not Found.");
            document.AddDomainEvent(new UpdatedEvent<Document>(document));
            if (request.UploadRequest != null)
            {
                document.URL = await _uploadService.UploadAsync(request.UploadRequest);
            }
            document.Title = request.Title;
            document.Description = request.Description;
            document.IsPublic = request.IsPublic;
            document.DocumentType = request.DocumentType;
            await _UnitOfWork.SaveChangesAsync(cancellationToken);
            return  MethodResult<int>.ResultWithData(document.Id);
        }
        else
        {
            var document = _Mapper.Map<Document>(dto);
            if (request.UploadRequest != null)
            {
                document.URL = await _uploadService.UploadAsync(request.UploadRequest); ;
            }
             document.AddDomainEvent(new CreatedEvent<Document>(document));
            _Repos.Insert(document);
            await _UnitOfWork.SaveChangesAsync(cancellationToken);
            return  MethodResult<int>.ResultWithData(document.Id);
        }
    }
}
