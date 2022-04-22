using System.Collections.Generic;

namespace Inventory.Api.v1.RequestModels
{
    public class SyncOutletRequestModel
    {
        public IEnumerable<SyncOutletItemRequestModel> OutletItems { get; set; }
    }
}
