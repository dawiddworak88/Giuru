using System;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class OrderFileLine
    {
        public string Sku { get; set; }
        public double Quantity { get; set; }
        public string ExternalReference { get; set; }
        public string MoreInfo { get; set; }
    }
}
