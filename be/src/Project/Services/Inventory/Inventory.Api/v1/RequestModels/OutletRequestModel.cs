using System.Collections.Generic;

namespace Inventory.Api.v1.RequestModels
{
    public class OutletRequestModel
    {
        public IEnumerable<OutletItemRequestModel> OutletItems { get; set; }
    }
}
