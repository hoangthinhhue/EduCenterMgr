using FluentValidation;
using UnitMgr.Domain.AggregatesModel.IdentityDTOs;

namespace UnitMgr.Admin.Pages.Identity.Users;
public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(v => v.TenantId.ToString())
             .MaximumLength(256)
             .NotEmpty();
        RuleFor(v => v.Provider)
             .MaximumLength(256)
             .NotEmpty();
        RuleFor(v => v.UserName)
             .MaximumLength(256)
             .NotEmpty();
        RuleFor(v => v.Email)
             .MaximumLength(256)
             .NotEmpty()
             .EmailAddress();

        RuleFor(p => p.Password).NotEmpty().WithMessage("Mật khẩu không thể trống")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.")
            .MaximumLength(16).WithMessage("Mật khẩu không thể quá 16 kýtự.")
            .Matches(@"[A-Z]+").WithMessage("Mật khẩu phải có ít nhật 1 ký tự hoa.")
            .Matches(@"[a-z]+").WithMessage("Mật khẩu phải có ít nhật 1 ký thường.")
            .Matches(@"[0-9]+").WithMessage("Mật khẩu phải có ít nhật 1 ký tự số.");
        RuleFor(x => x.ConfirmPassword)
             .Equal(x => x.Password);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<UserDto>.CreateWithOptions((UserDto)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}