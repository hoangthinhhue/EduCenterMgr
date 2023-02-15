// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Mgr.Core.Models;

namespace CleanArchitecture.Blazor.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedData<TDestination>> PaginatedDataAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
            => PaginatedData<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
            => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
}
