using Analytics.Api.ServicesModels.SalesAnalytics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Analytics.Api.Services.SalesAnalytics
{
    public interface ISalesService
    {
        Task<IEnumerable<TopSalesProductsAnalyticsServiceModel>> GetTopSalesProductsAnalyticsAsync(GetTopSalesProductsAnalyticsServiceModel model);
        Task<IEnumerable<AnnualSalesServiceModel>> GetAnnualSalesServiceModel(GetAnnualSalesServiceModel model);
        Task CreateAsync(CreateSalesAnalyticsServiceModel model);
    }
}
