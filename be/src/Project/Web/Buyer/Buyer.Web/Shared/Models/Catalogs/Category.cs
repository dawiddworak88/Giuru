using System;

namespace Buyer.Web.Shared.Models.Catalogs
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public int Level { get; set; }
    }
}
