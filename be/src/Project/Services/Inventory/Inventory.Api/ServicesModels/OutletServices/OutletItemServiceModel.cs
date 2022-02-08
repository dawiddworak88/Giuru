using System;

namespace Inventory.Api.ServicesModels.OutletServices
{
    public class OutletItemServiceModel
    {
        public Guid? ProductId { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
    }
}
