using UnitMgr.Admin.Models.SideMenu;

namespace UnitMgr.Admin.Services.Navigation;

public interface IMenuService
{
    IEnumerable<MenuSectionModel> Features { get; }
}
