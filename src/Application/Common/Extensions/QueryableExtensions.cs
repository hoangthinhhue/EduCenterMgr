// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Common;
using CleanArchitecture.Blazor.Domain.Interfaces;
using Mgr.Core.Interface;

namespace CleanArchitecture.Blazor.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class, IEntity
    {
        var queryableMethodResultWithIncludes = spec.Includes
           .Aggregate(query,
               (current, include) => current.Include(include));
        var secondaryMethodResult = spec.IncludeStrings
            .Aggregate(queryableMethodResultWithIncludes,
                (current, include) => current.Include(include));
        return secondaryMethodResult.Where(spec.Criteria);
    }
}
