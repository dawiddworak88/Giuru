using System;

namespace Inventory.Api.v1.RequestModels
{
    public class OutletItemRequestModel
    {
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
    }
}
