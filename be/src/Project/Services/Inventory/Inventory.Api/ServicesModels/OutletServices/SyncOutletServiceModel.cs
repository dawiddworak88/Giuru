using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.OutletServices
{
    public class SyncOutletServiceModel : BaseServiceModel
    {
        public IEnumerable<SyncOutletItemServiceModel> OutletItems { get; set; }
    }
}
