using Microsoft.AspNetCore.Components;
using MudBlazor;
using UnitMgr.Admin.Components.Dialogs;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using BlazorState;
using System.Security.Cryptography;
using UnitMgr.Application.Features.Identity.Profile;
using Mgr.Core.Models;
using UnitMgr.Domain.Constants;

namespace UnitMgr.Admin.Components.Shared;

public partial class UserMenu:  BlazorStateComponent
{
    UserProfileState UserProfileState => GetState<UserProfileState>();
    private UserProfile UserProfile => UserProfileState.UserProfile;
    [Parameter] public EventCallback<MouseEventArgs> OnSettingClick { get; set; }
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;
    private async Task OnLogout()
    {
        var parameters = new DialogParameters
            {
                { nameof(LogoutConfirmation.ContentText), $"{ConstantString.LOGOUTCONFIRMATION}"},
                { nameof(LogoutConfirmation.Color), Color.Error}
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, FullWidth = true };
        var dialog = DialogService.Show<LogoutConfirmation>(ConstantString.LOGOUTCONFIRMATIONTITLE, parameters, options);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            NavigationManager.NavigateTo("/auth/logout", true);
        }
    } 
}