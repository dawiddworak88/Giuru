using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.OutletServices
{
    public class OutletServiceModel : BaseServiceModel
    {
        public IEnumerable<OutletItemServiceModel> OutletItems { get; set; }
    }
}
