// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Mgr.Core.Interfaces.Services;
using Mgr.Core.Models;

namespace UnitMgr.Application.Features;

public interface IUploadService : IBaseService
{ 
    Task<string> UploadAsync(UploadRequest request);
}
