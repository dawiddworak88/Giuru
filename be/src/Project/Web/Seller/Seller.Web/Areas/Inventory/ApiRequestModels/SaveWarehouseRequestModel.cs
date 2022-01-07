using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Inventory.ApiRequestModels
{
    public class SaveWarehouseRequestModel : RequestModelBase
    {
        public string Name { get; set; }   
        public string Location { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
