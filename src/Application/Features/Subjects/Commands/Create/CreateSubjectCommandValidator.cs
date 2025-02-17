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
//       This file defines the validation rules for the CreateSubjectCommand, 
//       used to create Subject entities within the CleanArchitecture.Blazor 
//       application. It ensures that essential fields are validated correctly, 
//       including maximum lengths and mandatory requirements for required fields.
//     
//     Documentation:
//       https://docs.cleanarchitectureblazor.com/features/subject
// </auto-generated>
//------------------------------------------------------------------------------

// Usage:
// This validator is used to ensure that a CreateSubjectCommand meets the required
// validation criteria. It enforces constraints like maximum field lengths and 
// ensures that the Name field is not empty before proceeding with the command execution.

namespace CleanArchitecture.Blazor.Application.Features.Subjects.Commands.Create;

public class CreateSubjectCommandValidator : AbstractValidator<CreateSubjectCommand>
{
        public CreateSubjectCommandValidator()
        {
                RuleFor(v => v.Code).MaximumLength(255); 
    RuleFor(v => v.Name).MaximumLength(50).NotEmpty(); 
    RuleFor(v => v.Description).MaximumLength(255); 

        }
       
}

