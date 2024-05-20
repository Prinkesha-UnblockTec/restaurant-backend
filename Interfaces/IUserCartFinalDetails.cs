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
        ICollection<DeliveryAddress> GetAddressByUsernamePassword(string username, string password);
        //string? GetAddressByUsernamePassword(UserNameandPassword model);
    }
}
