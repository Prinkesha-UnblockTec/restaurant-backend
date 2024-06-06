using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IMasterRepository
    {
        ICollection<Master> GetAllMasterData();
        bool AddedMasterList(Master model);
        bool EditMaster(Master model);
        bool DeleteMaster(int id);
        ICollection<ActiveMaster> GetActiveMaster();
        ICollection<TableNoBaseTotalAmount> GetTotalAmountTableNo();
        ICollection<CartIDAndUserName> GetLastRecordByTableNo(string TableNo);
    }
}
