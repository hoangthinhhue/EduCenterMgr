

using BlazorState;
using Mgr.Core.Models;
using Microsoft.AspNetCore.Components;
using UnitMgr.Admin.Models.SideMenu;
using UnitMgr.Admin.UIServices;
using UnitMgr.Application.Features.Identity.Profile;

namespace UnitMgr.Admin.Components.Shared;

public partial class SideMenu:BlazorStateComponent,IDisposable
{
    UserProfileState UserProfileState => GetState<UserProfileState>();
    private UserProfile UserProfile => UserProfileState.UserProfile;

    [EditorRequired] [Parameter] 
    public bool SideMenuDrawerOpen { get; set; } 
    
    [EditorRequired] [Parameter]
    public EventCallback<bool> SideMenuDrawerOpenChanged { get; set; }

    [Inject] 
    private IMenuService _menuService { get; set; } = default!;
    private IEnumerable<MenuSectionModel> _menuSections => _menuService.Features;
    
    [Inject] 
    private LayoutService LayoutService { get; set; } = default!;

    private string[] _roles => UserProfile.AssignRoles??new string[] { };






}