﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This file is part of the CleanArchitecture.Blazor project.
//     Licensed to the .NET Foundation under one or more agreements.
//     The .NET Foundation licenses this file to you under the MIT license.
//     See the LICENSE file in the project root for more information.
//
//     Author: neozhu
//     Created Date: 2025-02-17
//     Last Modified: 2025-02-17
//     Description: 
//       This file defines the UpdateSubjectCommand and its handler for updating 
//       an existing Subject entity within the CleanArchitecture.Blazor application. 
//       It includes caching invalidation logic to maintain data consistency and 
//       raises a domain event upon successful update to notify other parts of the system.
//     
//     Documentation:
//       https://docs.cleanarchitectureblazor.com/features/subject
// </auto-generated>
//------------------------------------------------------------------------------

// Usage:
// Use `UpdateSubjectCommand` to update an existing subject entity in the system. 
// The handler ensures that if the entity is found, the changes are applied and 
// the necessary domain event (`SubjectUpdatedEvent`) is raised. Caching is also 
// invalidated to keep the subject list consistent.

using CleanArchitecture.Blazor.Application.Features.Subjects.Caching;
using CleanArchitecture.Blazor.Application.Features.Subjects.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Subjects.Commands.Update;

public class UpdateSubjectCommand: ICacheInvalidatorRequest<Result<int>>
{
      [Description("Id")]
      public int Id { get; set; }
            [Description("Code")]
    public string? Code {get;set;} 
    [Description("Name")]
    public string Name {get;set;} 
    [Description("Description")]
    public string? Description {get;set;} 

      public string CacheKey => SubjectCacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => SubjectCacheKey.Tags;

}

public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public UpdateSubjectCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {

       var item = await _context.Subjects.FindAsync(request.Id, cancellationToken);
       if (item == null)
       {
           return await Result<int>.FailureAsync($"Subject with id: [{request.Id}] not found.");
       }
       SubjectMapper.ApplyChangesFrom(request, item);
	    // raise a update domain event
	   item.AddDomainEvent(new SubjectUpdatedEvent(item));
       await _context.SaveChangesAsync(cancellationToken);
       return await Result<int>.SuccessAsync(item.Id);
    }
}

