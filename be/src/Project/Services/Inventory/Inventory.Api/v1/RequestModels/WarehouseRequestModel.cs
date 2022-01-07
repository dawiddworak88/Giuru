using Foundation.ApiExtensions.Models.Request;
using System;

namespace Inventory.Api.v1.RequestModels
{
    public class WarehouseRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
