// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;

namespace Mgr.Core.Models;

public class PaginatedData<T>
{
    public int CurrentPage { get; private set; }
    public int TotalItems { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public IList<T> Data { get; set; }
    public PaginatedData(IList<T> items, int total, int pageIndex, int pageSize)
    {
        Data = items;
        TotalItems = total;
        CurrentPage = pageIndex;
        TotalPages = (int)Math.Ceiling(total / (double)pageSize);
    }
    public static async Task<PaginatedData<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedData<T>(items, count, pageIndex, pageSize);
    }
    public static PaginatedData<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count =  source.Count();
        var items =  source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedData<T>(items, count, pageIndex, pageSize);
    }
}
