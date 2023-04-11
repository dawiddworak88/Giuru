using System;

namespace Seller.Web.Areas.Dashboard.ApiResponseModels
{
    public class ClientSalesAnalyticsResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
    }
}
