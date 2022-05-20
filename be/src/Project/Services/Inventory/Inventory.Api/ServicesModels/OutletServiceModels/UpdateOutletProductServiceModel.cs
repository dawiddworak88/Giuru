using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.OutletServiceModels
{
    public class UpdateOutletProductServiceModel : BaseServiceModel
    {
        public Guid? WarehouseId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public double Quantity { get; set; }
        public double AvailableQuantity { get; set; }
        public string ProductEan { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
