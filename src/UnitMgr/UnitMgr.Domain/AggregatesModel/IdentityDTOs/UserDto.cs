// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using FluentValidation;
using Mgr.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using UnitMgr.Domain.Configs.Mappings;

namespace UnitMgr.Domain.AggregatesModel.IdentityDTOs;

public class UserDto : IMapFrom<ApplicationUser>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ApplicationUser, UserDto>(MemberList.None)
           .ForMember(x => x.AssignedRoles, s => s.MapFrom(y => y.UserRoles.Select(r => r.Role.Name)));
        profile.CreateMap<UserDto, ApplicationUser>();
    }
    public string? Code { get; set; }
    public string[]? AssignedRoles { get; set; }
    public Guid Id { get; set; } =Guid.Empty;
    public string UserName { get; set; }  
    public string? DisplayName { get; set; }
    public string? Provider { get; set; } = "Local";
    public Guid? TenantId { get; set; }
    public string? TenantName { get; set; }
    public string? ProfilePictureDataUrl { get; set; }
    public bool IsActive { get; set; }
    public bool IsLive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Checked { get; set; }
    public string? Role { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }}
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
             .MinimumLength(2)
             .MaximumLength(100)
             .NotEmpty().WithMessage("Tên tài khoản không thể trống");
        RuleFor(v => v.Email)
             .MaximumLength(256)
             .NotEmpty().WithMessage("Email không thể trống")
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
