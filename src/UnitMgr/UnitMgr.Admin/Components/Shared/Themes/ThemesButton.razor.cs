using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using UnitMgr.Admin.UIServices;

namespace UnitMgr.Admin.Components.Shared.Themes;

public partial class ThemesButton
{
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    
    [Inject] private LayoutService LayoutService { get; set; } = default!;

}