using UnitMgr.Admin.Models.SideMenu;

namespace UnitMgr.Admin.UIServices;

public interface IMenuService
{
    IEnumerable<MenuSectionModel> Features { get; }
}
