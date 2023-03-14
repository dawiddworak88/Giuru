using System;

namespace Seller.Web.Areas.Dashboard.DomainModels
{
    public class ClientSalesApiItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
    }
}
