using Foundation.ApiExtensions.Models.Request;
using System;

namespace Inventory.Api.v1.RequestModels
{
    public class OutletRequestModel : RequestModelBase
    {
        public Guid? WarehouseId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public string Ean { get; set; }
        public double AvailableQuantity { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
