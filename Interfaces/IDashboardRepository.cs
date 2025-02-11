﻿using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IDashboardRepository
    {
        StausWiseShowOrder GetStatusOrderDetails();
        ICollection<TotalItemRecord> GetTotalItemRecord();
        ICollection<PaymentChartDatas> GetPaymentData(PaymentChartData model);
        ICollection<PaymentChartDatas> GetAllPaymentData();
        ICollection<DayWiseTotalAmount> GetDayWiseTotalAmount(OnlyDates model);
        ICollection<GetTopSellingItems> GetTopSellingItems(TopSellingItemsParameters model);
        ICollection<TotalCategorywithItemSale> GetTotalCategorywithItemSale();
        Task<ICollection<OrderSummary>> GetOrderSummaries(int? year = null);
        List<OrderDetails> GetFilteredOrderDetails(OrderFilterModel orderFilter);
    }
}
