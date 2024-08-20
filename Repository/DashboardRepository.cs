using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace restaurant.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment environment;

        public DashboardRepository(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            _configuration = configuration;
        }
        public StausWiseShowOrder GetStatusOrderDetails()
        {
            var StausWiseShowOrderList = new List<ShowOrders>();
            int totalRecords = 0;
            int totalAmount = 0;
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetStatusWiseOrderCount", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ShowOrders items = new ShowOrders
                        {
                            TotalOrder = reader["TotalOrders"] != DBNull.Value ? Convert.ToInt32(reader["TotalOrders"]) : 0,
                            TotalAmount = reader["TotalAmount"] != DBNull.Value ? Convert.ToInt32(reader["TotalAmount"]) : 0,
                            Status = reader["Status"].ToString(),
                        };
                        StausWiseShowOrderList.Add(items);

                    }
                    if (reader.NextResult())
                    {
                        // Read the second result set (total record count)
                        if (reader.Read())
                        {
                            totalRecords = reader["TotalRecords"] != DBNull.Value ? Convert.ToInt32(reader["TotalRecords"]) : 0;
                            totalAmount = reader["TotalAmount"] != DBNull.Value ? Convert.ToInt32(reader["TotalAmount"]) : 0;
                        }
                    }
                }
            }
            return new StausWiseShowOrder
            {
                ShowOrderss = StausWiseShowOrderList,
                TotalRecords = totalRecords,
                TotalAmount = totalAmount,
            };
            //return StausWiseShowOrderList;
        }

        public ICollection<TotalItemRecord> GetTotalItemRecord()
        {
            var TotalItemRecordList = new List<TotalItemRecord>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllItemDetails", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TotalItemRecord items = new TotalItemRecord
                        {
                            TotalQuantity = Convert.ToInt32(reader["TotalQuantity"]),
                            TotalPrice = Convert.ToInt32(reader["TotalPrice"]),
                            ItemName = reader["ItemName"].ToString(),
                        };
                        TotalItemRecordList.Add(items);

                    }
                }
            }
            return TotalItemRecordList;
        }

        public ICollection<TotalCategorywithItemSale> GetTotalCategorywithItemSale()
        {
            var TotalItemRecordList = new List<TotalCategorywithItemSale>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetCategoriesWiseItemSales", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TotalCategorywithItemSale items = new TotalCategorywithItemSale
                        {
                            MonthYear = reader["MonthYear"].ToString(),
                            TotalPrice = Convert.ToInt32(reader["TotalPrice"]),
                            Category = reader["Category"].ToString(),
                        };
                        TotalItemRecordList.Add(items);

                    }
                }
            }
            return TotalItemRecordList;
        }
        public async Task<ICollection<OrderSummary>> GetOrderSummaries(int? year = null)
        {
            var orderSummaries = new List<OrderSummary>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetOrderSummaries", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (year.HasValue)
                {
                    cmd.Parameters.Add(new SqlParameter("@Year", year.Value));
                }

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        OrderSummary summary = new OrderSummary
                        {
                            Period = reader["Period"].ToString(),
                            TotalOrders = reader["TotalOrders"] != DBNull.Value ? Convert.ToInt32(reader["TotalOrders"]) : 0,
                            TotalPrice = reader["TotalPrice"] != DBNull.Value ? Convert.ToInt32(reader["TotalPrice"]) : 0
                        };
                        orderSummaries.Add(summary);
                    }
                }
            }
            return orderSummaries;
        }
        public List<OrderDetails> GetFilteredOrderDetails(OrderFilterModel orderFilter)
        {
            var orderDetails = new List<OrderDetails>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetFilteredOrderDetails", con))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Convert item list to a comma-separated string
                    var itemListString = string.Join(",", orderFilter.ItemList);

                    // Add parameters
                    command.Parameters.AddWithValue("@ItemList", itemListString);
                    command.Parameters.AddWithValue("@FromDate", orderFilter.FromDate);
                    command.Parameters.AddWithValue("@ToDate", orderFilter.ToDate);

                    con.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderDetails.Add(new OrderDetails
                            {
                                Month = reader["Month"].ToString(),
                                TotalPrice = Convert.ToInt32(reader["TotalPrice"]),
                                TotalQuantity = Convert.ToInt32(reader["TotalQuantity"])
                            });
                        }
                    }
                }
            }

            return orderDetails;
        }

        public ICollection<DayWiseTotalAmount> GetDayWiseTotalAmount(OnlyDates model)
        {
            var DayWiseTotalAmount = new List<DayWiseTotalAmount>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDayWiseTotalAmount", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@FromDate", model.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", model.ToDate);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DayWiseTotalAmount items = new DayWiseTotalAmount
                        {
                            DayOfWeek = reader["DayOfWeek"].ToString(),
                            TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                            TotalQuantity = Convert.ToInt32(reader["TotalQuantity"]),
                        };
                        DayWiseTotalAmount.Add(items);

                    }
                }
            }
            return DayWiseTotalAmount;
        }

        public ICollection<GetTopSellingItems> GetTopSellingItems(TopSellingItemsParameters model)
        {
            var TopSellingItems = new List<GetTopSellingItems>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetTopSellingItems", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@FromDate", model.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", model.ToDate);
                cmd.Parameters.AddWithValue("@TopOrBottom", model.TopOrBottom);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GetTopSellingItems items = new GetTopSellingItems
                        {
                            ItemName = reader["ItemName"].ToString(),
                            TotalQuantity = Convert.ToInt32(reader["TotalQuantity"]),
                            RowNumber = Convert.ToInt32(reader["RowNumber"]),
                        };
                        TopSellingItems.Add(items);

                    }
                }
            }
            return TopSellingItems;
        }

        public ICollection<PaymentChartDatas> GetPaymentData(PaymentChartData model)
        {
            var PaymentItems = new List<PaymentChartDatas>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetPaymentSummaryByDateRange", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StartDateString", model.StartDate);
                cmd.Parameters.AddWithValue("@EndDateString", model.EndDate);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PaymentChartDatas items = new PaymentChartDatas
                        {
                            PaymentType = reader["PaymentType"] != DBNull.Value ? reader["PaymentType"].ToString() : string.Empty,
                            Price = reader["TotalPrice"] != DBNull.Value ? Convert.ToInt32(reader["TotalPrice"]) : 0,
                        };
                        PaymentItems.Add(items);

                    }
                }
            }
            return PaymentItems;
        }

        public ICollection<PaymentChartDatas> GetAllPaymentData()
        {
            var PaymentItems = new List<PaymentChartDatas>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetPaymentSummary", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PaymentChartDatas items = new PaymentChartDatas
                        {
                            Date = reader["MonthYear"] != DBNull.Value ? reader["MonthYear"].ToString() : string.Empty,
                            PaymentType = reader["PaymentType"] != DBNull.Value ? reader["PaymentType"].ToString() : string.Empty,
                            Price = reader["TotalPrice"] != DBNull.Value ? Convert.ToInt32(reader["TotalPrice"]) : 0,
                        };
                        PaymentItems.Add(items);

                    }
                }
            }
            return PaymentItems;
        }
    }
}
