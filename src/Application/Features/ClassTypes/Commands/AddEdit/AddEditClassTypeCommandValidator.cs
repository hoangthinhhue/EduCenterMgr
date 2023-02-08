// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.Commands.AddEdit;

public class AddEditClassTypeCommandValidator : AbstractValidator<AddEditClassTypeCommand>
{
    public AddEditClassTypeCommandValidator()
    {
       
        RuleFor(v => v.Name)
              .MaximumLength(500)
              .NotEmpty();
        RuleFor(v => v.Code)
            .MaximumLength(50);
        RuleFor(v => v.Duration)
               .GreaterThanOrEqualTo(0);
        RuleFor(v => v.Description)
                   .MaximumLength(1024);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<AddEditClassTypeCommand>.CreateWithOptions((AddEditClassTypeCommand)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}

