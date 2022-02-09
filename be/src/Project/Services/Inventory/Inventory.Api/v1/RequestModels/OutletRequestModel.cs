using Foundation.ApiExtensions.Models.Request;
using System;

namespace Inventory.Api.v1.RequestModels
{
    public class OutletRequestModel : RequestModelBase
    {
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int? Quantity { get; set; }
    }
}
