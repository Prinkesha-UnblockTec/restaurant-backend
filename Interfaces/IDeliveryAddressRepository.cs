using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IDeliveryAddressRepository
    {
        ICollection<DeliveryAddresses> GetAllDeliveryAddressesData(int loginId);
        ICollection<DeliveryAddresses> GetDefaultAddress(int loginId);
        bool AddedDeliveryAddressesList(DeliveryAddresses model);
        bool EditDeliveryAddresses(DeliveryAddresses model);
        bool SetDefaultAddress(SetDefult model);
        bool DeleteDeliveryAddresses(int id);
    }
}
