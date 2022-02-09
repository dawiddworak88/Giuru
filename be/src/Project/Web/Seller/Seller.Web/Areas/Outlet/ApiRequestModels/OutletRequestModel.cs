using System;

namespace Seller.Web.Areas.Outlet.ApiRequestModels
{
    public class OutletRequestModel
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
    }
}
