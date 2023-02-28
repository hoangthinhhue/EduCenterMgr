using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Mgr.Core.Entities;

namespace UnitMgr.Domain.AggregatesModel.TenantDTOs;
public class TenantValidator : AbstractValidator<Tenant>
{
    public TenantValidator()
    {
        RuleFor(v => v.Name)
               .MaximumLength(256)
               .NotEmpty();
        RuleFor(v => v.Description)
               .MaximumLength(500);
    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Tenant>
            .CreateWithOptions((Tenant)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}


