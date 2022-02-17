using System;

namespace Inventory.Api.ServicesModels.OutletServices
{
    public class SyncOutletItemServiceModel
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
