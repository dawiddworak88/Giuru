using Foundation.ApiExtensions.Models.Request;
using System.Collections.Generic;

namespace Inventory.Api.v1.RequestModels
{
    public class SaveOutletsBySkusRequestModel
    {
        public IEnumerable<UpdateOutletProductRequestModel> OutletItems { get; set; }
    }
}
