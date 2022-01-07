using System;

namespace Seller.Web.Areas.Inventory.DomainModels
{
    public class Warehouse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
