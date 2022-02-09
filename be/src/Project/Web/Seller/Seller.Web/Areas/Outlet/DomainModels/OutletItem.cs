using System;

namespace Seller.Web.Areas.Outlet.DomainModels
{
    public class OutletItem
    {
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
