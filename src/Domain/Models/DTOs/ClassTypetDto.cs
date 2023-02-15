// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Domain.Interfaces.Mappings;

namespace CleanArchitecture.Blazor.Domain.DTOs.ClassTypes.DTOs;


public class ClassTypeDto : IMapFrom<ClassType>
    {

    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    /// <summary>
    /// Số giờ giảng dạy dự kiến
    /// </summary>
    public int? Duration { get; set; }

}

