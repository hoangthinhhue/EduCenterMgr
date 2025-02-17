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
//       This file defines the validation rules for the DeleteSubjectCommand used
//       to delete Subject entities within the CleanArchitecture.Blazor application. 
//       It ensures that the command has valid input, particularly verifying that the 
//       list of subject IDs to delete is not null and contains only positive IDs.
//     
//     Documentation:
//       https://docs.cleanarchitectureblazor.com/features/subject
// </auto-generated>
//------------------------------------------------------------------------------

// Usage:
// This validator ensures that the DeleteSubjectCommand is valid before attempting 
// to delete subject records from the system. It verifies that the ID list is not 
// null and that all IDs are greater than zero.

namespace CleanArchitecture.Blazor.Application.Features.Subjects.Commands.Delete;

public class DeleteSubjectCommandValidator : AbstractValidator<DeleteSubjectCommand>
{
        public DeleteSubjectCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

