using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IMenuAccessRepository
    {
        
      bool EditMenuAccessDatas(MonuAccess model);
        ICollection<MonuAccess> GetAllMenuAccessData();

    }
}
