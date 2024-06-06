using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IDashboardRepository
    {
        StausWiseShowOrder GetStatusOrderDetails();
        ICollection<TotalItemRecord> GetTotalItemRecord();
        ICollection<TotalCategorywithItemSale> GetTotalCategorywithItemSale();
        Task<ICollection<OrderSummary>> GetOrderSummaries(int? year = null);
        List<OrderDetails> GetFilteredOrderDetails(OrderFilterModel orderFilter);
    }
}
