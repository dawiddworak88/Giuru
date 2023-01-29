using Analytics.Api.ServicesModels.SalesAnalytics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Analytics.Api.Services.SalesAnalytics
{
    public interface ISalesService
    {
        IEnumerable<TopSalesProductsAnalyticsServiceModel> GetTopSalesProductsAnalyticsAsync(GetTopSalesProductsAnalyticsServiceModel model);
        IEnumerable<AnnualSalesServiceModel> GetAnnualSalesServiceModel(GetAnnualSalesServiceModel model);
        Task CreateAsync(CreateSalesAnalyticsServiceModel model);
    }
}
