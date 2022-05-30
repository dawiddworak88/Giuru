using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.OutletServiceModels
{
    public class GetOutletByProductIdServiceModel : BaseServiceModel
    {
        public Guid? ProductId { get; set; }
    }
}
