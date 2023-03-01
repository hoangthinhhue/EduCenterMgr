using FluentValidation;

namespace UnitMgr.Admin.Pages.Identity.Users;

public class ChangePasswordModel
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
{
    public ChangePasswordModelValidator()
    {
        RuleFor(p => p.NewPassword).NotEmpty().WithMessage("Mật khẩu không thể trống")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.")
            .MaximumLength(16).WithMessage("Mật khẩu không thể quá 16 kýtự.")
            .Matches(@"[A-Z]+").WithMessage("Mật khẩu phải có ít nhật 1 ký tự hoa.")
            .Matches(@"[a-z]+").WithMessage("Mật khẩu phải có ít nhật 1 ký thường.")
            .Matches(@"[0-9]+").WithMessage("Mật khẩu phải có ít nhật 1 ký tự số.");
        RuleFor(x => x.ConfirmPassword)
             .Equal(x => x.NewPassword);

    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<ChangePasswordModel>.CreateWithOptions((ChangePasswordModel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}

