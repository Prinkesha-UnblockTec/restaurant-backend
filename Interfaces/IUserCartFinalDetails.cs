using  restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IUserCartFinalDetails
    {
        ICollection<Items> GetUserCartProductsByTableName(ItemDataBseOnUser user);
        ICollection<AllCartItems> GetAllCartItems();
        ICollection<Notification> GetAllNotificationData();
        ICollection<Payment> GetPaymentAllData();
        ICollection<OrdersAdmin> GetOrdersInAdminData(int ID);
        bool UpdateCheckedItems(UpdateCheckedItems model);
        bool UpdatePaymentType(PaymentUpdate model);
        bool UpdateParticularUserStatus(UpdateStatusUserCart model);
        bool UpdateNotificationRoleName(UpdateRoleNameForNotification model);
        bool UpdateNotificationIsRead(UpdateRoleNameForNotification model);
        bool DeleteCompleteOrderforNotification(int Id);
        bool DeleteCheckedRTecordsInNotifications(string Id);
        bool UpdateNotification(Notification model);
        bool AddedUserCartList(UserCartFinalDetails.CartDetails model);
        bool StoreSatausData(StoreSatausData model);
        void AssignChefsAndUpdateDetails(ModifyrelatableChef model);
        ICollection<StoreSatausData> GetStoreSatausData(int ID);
        ICollection<DeliveryAddress> GetAddressByUsernamePassword(string username, string password);
        ICollection<DeliveryAddress> GetAddressByOrderId(int id);
       string GetStatusUser(int id);
        string GetChefNameById(int id);
        int GetTotalAmountByCartId(int cartId);
        string GetLastSelectedOrderType();
        bool AddedDataBaseCartId(CartModel model);
        //ICollection<DeliveryAddress> GetExistingOrderIdBasEONtABLE();
        //string? GetAddressByUsernamePassword(UserNameandPassword model);
    }
}
