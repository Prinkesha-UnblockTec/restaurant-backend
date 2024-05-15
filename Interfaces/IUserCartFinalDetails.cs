using  restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IUserCartFinalDetails
    {
        ICollection<Items> GetUserCartProductsByTableName(ItemDataBseOnUser user);
        ICollection<AllCartItems> GetAllCartItems();
        bool UpdateParticularUserStatus(UpdateStatusUserCart model);
        bool AddedUserCartList(UserCartFinalDetails.CartDetails model);
        string GetAddressByUsernamePassword(string username, string password);
        //string? GetAddressByUsernamePassword(UserNameandPassword model);
    }
}
