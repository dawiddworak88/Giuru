using System.Collections.Generic;

namespace Analytics.Api.v1.RequestModels
{
    public class SalesAnalyticsRequestModel
    {
        public IEnumerable<SalesAnalyticsItemRequestModel> SalesAnalyticsItems { get; set; }
    }
}
