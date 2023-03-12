using Analytics.Api.ServicesModels.SalesAnalytics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Analytics.Api.Services.SalesAnalytics
{
    public interface ISalesService
    {
        IEnumerable<AnnualSalesServiceModel> GetAnnualSales(GetAnnualSalesServiceModel model);
        IEnumerable<CountrySalesServiceModel> GetCountrySales(GetCountriesSalesServiceModel model);
        IEnumerable<DailySalesServiceModel> GetDailySales(GetDailySalesServiceModel model);
        IEnumerable<TopSalesProductsAnalyticsServiceModel> GetTopProductsSales(GetTopSalesProductsAnalyticsServiceModel model);
        IEnumerable<ClientSalesServiceModel> GetTopClientsSales(GetClientsSalesServiceModel model);
        Task CreateAsync(CreateSalesAnalyticsServiceModel model);
    }
}
