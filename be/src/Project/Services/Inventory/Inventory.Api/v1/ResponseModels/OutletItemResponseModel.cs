using System;

namespace Inventory.Api.v1.ResponseModels
{
    public class OutletItemResponseModel
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int? Quantity { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
