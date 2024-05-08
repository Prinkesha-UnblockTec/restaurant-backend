using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IRoleRepository
    {
        ICollection<Role> GetAllRoleData();
        bool AddedRoleList(Role model);
        bool EditRole(Role model);
        bool DeleteRole(int id);
        ICollection<ActiveRole> GetActiveRole();
    }
}
