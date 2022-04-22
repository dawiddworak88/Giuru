using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.OutletServiceModels
{
    public class UpdateBasketOutletServiceModel : BaseServiceModel
    {
        public Guid? ProductId { get; set; }
        public int BookedQuantity { get; set; }
    }
}
