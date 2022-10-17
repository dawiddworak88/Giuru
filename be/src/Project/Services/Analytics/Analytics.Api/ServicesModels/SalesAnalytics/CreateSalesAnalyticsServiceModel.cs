using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class CreateSalesAnalyticsServiceModel : BaseServiceModel
    {
        public IEnumerable<CreateSalesAnalyticsItemServiceModel> SalesAnalyticsItems { get; set; }
    }
}
