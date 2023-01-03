using Analytics.Api.ServicesModels.SalesAnalytics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Analytics.Api.Services.SalesAnalytics
{
    public interface ISalesService
    {
        Task CreateAsync(CreateSalesAnalyticsServiceModel model);
        Task<IEnumerable<TopSalesProductsAnalyticsServiceModel>> GetTopSalesProductsAnalyticsAsync(GetTopSalesProductsAnalyticsServiceModel model);
        Task<IEnumerable<AnnualSalesServiceModel>> GetAnnualSalesServiceModel(GetAnnualSalesServiceModel model);
    }
}
