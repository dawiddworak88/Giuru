using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels
{
    public class WarehouseServiceModel 
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
