using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.OutletServiceModels
{
    public class UpdateOutletProductsServiceModel : BaseServiceModel
    {
        public IEnumerable<UpdateOutletProductServiceModel> OutletItems { get; set; }
    }
}
