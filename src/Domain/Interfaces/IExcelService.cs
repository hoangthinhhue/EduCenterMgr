// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Mgr.Core.Entities;

namespace CleanArchitecture.Blazor.Domain.Interfaces;

public interface IExcelService
{
    Task<byte[]> CreateTemplateAsync(IEnumerable<string> fields, string sheetName = "Sheet1");
    Task<byte[]> ExportAsync<TData>(IEnumerable<TData> data
        , Dictionary<string, Func<TData, object?>> mappers
, string sheetName = "Sheet1");

    Task<MethodResult<IEnumerable<TEntity>>> ImportAsync<TEntity>(byte[] data
        , Dictionary<string, Func<DataRow, TEntity, object?>> mappers
        , string sheetName = "Sheet1");
}
