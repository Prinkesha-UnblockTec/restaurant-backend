using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IChefMaster
    {
        ICollection<ChefMaster> GetAllChefMasterData();
        bool AddedChefMasterList(ChefMaster model);
        bool EditChefMaster(ChefMaster model);
        bool DeleteChefMaster(int id);
        ICollection<ActiveChef> GetActiveChefMaster();
    }
}
