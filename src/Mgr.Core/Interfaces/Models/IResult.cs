// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace Mgr.Core.Interfaces;

public interface IMethodResult
{
    bool Success { get; set; }

    string Message { get; set; }
    [JsonIgnore]
    int? Status { get; set; }
}

public interface IMethodResult<T> : IMethodResult
{
    T Data { get; set; }
    int TotalRecord { get; set; }
}
