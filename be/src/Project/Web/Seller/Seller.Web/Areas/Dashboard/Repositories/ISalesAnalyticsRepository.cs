using Seller.Web.Areas.Dashboard.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.Repositories
{
    public interface ISalesAnalyticsRepository
    {
        Task<IEnumerable<DailySalesItem>> GetDailySales(string token, string language, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<CountrySalesItem>> GetCountriesSales(string token, string language, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ProductSalesApiItem>> GetTopProductsSales(string token, string language, DateTime fromDate, DateTime toDate, int? size);
        Task<IEnumerable<ClientSalesApiItem>> GetTopClientsSales(string token, string language, DateTime fromDate, DateTime toDate, int? size);
    }
}
