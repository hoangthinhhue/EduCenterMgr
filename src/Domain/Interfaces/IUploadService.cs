// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Mgr.Core.Models;

namespace CleanArchitecture.Blazor.Domain.Interfaces;

public interface IUploadService
{
    Task<string> UploadAsync(UploadRequest request);
}
