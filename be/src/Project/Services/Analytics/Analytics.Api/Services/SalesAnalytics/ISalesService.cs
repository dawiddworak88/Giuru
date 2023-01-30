using Analytics.Api.ServicesModels.SalesAnalytics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Analytics.Api.Services.SalesAnalytics
{
    public interface ISalesService
    {
        IEnumerable<AnnualSalesServiceModel> GetAnnualSales(GetAnnualSalesServiceModel model);
        IEnumerable<CountrySalesServiceModel> GetCountrySales(GetCountriesSalesServiceModel model);
        Task CreateAsync(CreateSalesAnalyticsServiceModel model);
    }
}
