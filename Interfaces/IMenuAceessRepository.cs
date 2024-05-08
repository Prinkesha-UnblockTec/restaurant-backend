using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IMenuAceessRepository
    {
        ICollection<MenuAccess> GetAllMenuAccessData();
        bool SaveMenuAccessData(List<MenuAccess> model);
        ICollection<ActiveMenuAccess> GetDatasBaseOnRoleID(int RoleId);
        ICollection<MenuAccess> GetMenuListByRoleID(int RoleId);
        ICollection<Menu> GetActiveMenu();
    }
}
