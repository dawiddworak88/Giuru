using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.OutletServices
{
    public class UpdateOutletServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
    }
}
