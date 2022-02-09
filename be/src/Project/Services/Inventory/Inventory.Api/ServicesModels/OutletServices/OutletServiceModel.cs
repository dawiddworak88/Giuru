using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.OutletServices
{
    public class OutletServiceModel : BaseServiceModel
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
