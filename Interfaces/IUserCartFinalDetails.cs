using  restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IUserCartFinalDetails
    {
        ICollection<Items> GetUserCartProductsByTableName(ItemDataBseOnUser user);
        ICollection<AllCartItems> GetAllCartItems();
        ICollection<OrdersAdmin> GetOrdersInAdminData(int ID);
        bool UpdateParticularUserStatus(UpdateStatusUserCart model);
        bool AddedUserCartList(UserCartFinalDetails.CartDetails model);
        bool StoreSatausData(StoreSatausData model);
        ICollection<StoreSatausData> GetStoreSatausData(int ID);
        ICollection<DeliveryAddress> GetAddressByUsernamePassword(string username, string password);
        ICollection<DeliveryAddress> GetAddressByOrderId(int id);
       string GetStatusUser(int id);
        //string? GetAddressByUsernamePassword(UserNameandPassword model);
    }
}
